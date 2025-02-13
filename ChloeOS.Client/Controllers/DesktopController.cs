using Microsoft.AspNetCore.Mvc;

namespace ChloeOS.Client.Controllers;

public class DesktopController : Controller {

    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet("Error/{statusCode:alpha}")]
    public IActionResult Error(string statusCode) => View($"Error/{statusCode}");

}