using Ardalis.Result;
using System.Security.Claims;
using ChloeOS.Client.Models;
using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.FS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Directory = ChloeOS.Core.Models.FS.Directory;

namespace ChloeOS.Client.Controllers;

[Authorize]
public class DesktopController : Controller {

    private readonly IFileRepository _fileRepository;
    private readonly IDirectoryRepository _directoryRepository;

    public DesktopController(IFileRepository fileRepository, IDirectoryRepository directoryRepository) {
        _fileRepository = fileRepository;
        _directoryRepository = directoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index() {
        Directory? desktopDirectory;

        Result<Directory[]> getDesktopDirectoryResult = await _directoryRepository.GetByNameAsync("Desktop");
        if (getDesktopDirectoryResult.IsNotFound()) {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid ownerId)) {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Unable to get local user's ID. Please try again later."
                );
            }
            // Create it then!
            desktopDirectory = new Directory {
                Name = "Desktop",
                OwnerId = ownerId
            };

            // Attempt to create the folder.
            Result<Directory> createDesktopDirectoryResult = await _directoryRepository.CreateAsync(desktopDirectory);
            if (!createDesktopDirectoryResult.IsSuccess) {
                return StatusCode(StatusCodes.Status500InternalServerError, createDesktopDirectoryResult.Errors);
            }

            desktopDirectory = createDesktopDirectoryResult.Value;
        } else {
            desktopDirectory = getDesktopDirectoryResult.Value.First(d => d.Name == "Desktop");
        }

        // Get files from the "Desktop" folder.
        BrowsableContent content = (BrowsableContent) desktopDirectory;
        return View(content);
    }

    [HttpGet("Error/{statusCode:alpha}")]
    [AllowAnonymous]
    public IActionResult Error(string statusCode) => View($"Error/{statusCode}");

}