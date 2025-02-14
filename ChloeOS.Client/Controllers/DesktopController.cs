using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChloeOS.Client.Controllers;

[Authorize]
public class DesktopController : Controller {

    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet("Error/{statusCode:alpha}")]
    [AllowAnonymous]
    public IActionResult Error(string statusCode) => View($"Error/{statusCode}");

}