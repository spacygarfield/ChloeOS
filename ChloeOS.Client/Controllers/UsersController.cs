using System.Security.Claims;
using ChloeOS.Client.Models;
using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.Misc;
using ChloeOS.Core.Models.OS;
using Jane;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChloeOS.Client.Controllers;

[Route("user")]
public class UsersController : Controller {

    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository) => _userRepository = userRepository;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn() {
        IResult<User[]> users = await _userRepository.GetAllAsync();

        ViewData["Users"] = users;

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInCredentials credentials) {
        if (!ModelState.IsValid) {
            return View(credentials);
        }

        // Get requested user.
        IResult<User> getUserResult = await _userRepository.GetByUsernameAsync(credentials.Username.Trim());
        if (!getUserResult.Ok) {
            ModelState.AddModelError(string.Empty, getUserResult.Reason);
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
            new (ClaimTypes.Name, user.FullName),
            new (ClaimTypes.Surname, user.FirstName),
            new (ClaimTypes.GivenName, user.LastName),
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