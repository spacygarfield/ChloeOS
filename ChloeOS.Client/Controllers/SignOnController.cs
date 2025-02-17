using Ardalis.Result;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChloeOS.Client.Models;
using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.Misc;
using ChloeOS.Core.Models.OS;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ChloeOS.Client.Controllers;

[Authorize(Policy = "Signed-Out")]
public class SignOnController : Controller {

    private readonly IUserRepository _localUserRepository;

    public SignOnController(IUserRepository localUserRepository) => _localUserRepository = localUserRepository;

    [HttpGet]
    public async Task<IActionResult> Index() {
        Result<User[]> getUsersResult = await _localUserRepository.GetAllAsync();
        if (getUsersResult.IsUnavailable()) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, getUsersResult.Errors);
        }
        User[] users = getUsersResult.Value;

        ViewData["Users"] = new SelectList(users, "Username", "FullName");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SignInCredentials credentials) {
        Result<User[]> getUsersResult = await _localUserRepository.GetAllAsync();
        if (getUsersResult.IsUnavailable()) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, getUsersResult.Errors);
        }
        User[] users = getUsersResult.Value;

        ViewData["Users"] = new SelectList(users, "Username", "FullName");

        ModelState.Remove(nameof(SignInCredentials.Password));
        if (!ModelState.IsValid) {
            return View(credentials);
        }

        // Get requested user.
        Result<User> getUserResult = await _localUserRepository.GetByUsernameAsync(credentials.Username.Trim());
        if (getUserResult.IsNotFound()) {
            ModelState.AddModelError(string.Empty, getUserResult.Errors.First());
            return View(credentials);
        }

        TempData["User"] = JsonConvert.SerializeObject(getUserResult.Value);

        return RedirectToAction("User");
    }

    [HttpGet]
    [ActionName("User")]
    public IActionResult SelectedUser() {
        User user;

        try {
            user = JsonConvert.DeserializeObject<User>(TempData["User"]?.ToString()!)!;
        } catch {
            ModelState.AddModelError(string.Empty, "Please select a local user to sign into.");
            return RedirectToAction("Index");
        }

        // Setup view model.
        SignInCredentials credentials = new () { Username = user.Username };
        return View(credentials);
    }

    [HttpPost]
    [ActionName("User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SelectedUser(SignInCredentials credentials) {
        if (!ModelState.IsValid) {
            return View(credentials);
        }

        // Get requested user.
        Result<User> getUserResult = await _localUserRepository.GetByUsernameAsync(credentials.Username.Trim());
        if (getUserResult.IsNotFound()) {
            ModelState.AddModelError(string.Empty, getUserResult.Errors.First());
            return View(credentials);
        }
        User user = getUserResult.Value;

        // Hash password and compare if it is right.
        Password userPassword = (Password) user;
        if (!userPassword.VerifyHash(credentials.Password)) {
            ModelState.AddModelError(nameof(SignInCredentials.Password), "The password is incorrect. Please try again.");
            return View(credentials);
        }

        // Sign-In attempt.
        Claim[] claims = [
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.FullName),
            new (ClaimTypes.GivenName, user.FirstName),
            new (ClaimTypes.Surname, user.LastName),
        ];
        ClaimsIdentity identity = new (claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new (identity);
        AuthenticationProperties properties = new() {
            IsPersistent = true,
            IssuedUtc = DateTime.UtcNow,
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

        return RedirectToAction("Index", "Desktop");
    }

}