using System.Security.Claims;
using ChloeOS.Client.Models;
using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.FS;
using Jane;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChloeOS.Client.Controllers;

[Authorize]
public class DesktopController : Controller {

    private readonly IFileRepository _fileRepository;
    private readonly IFolderRepository _folderRepository;

    public DesktopController(IFileRepository fileRepository, IFolderRepository folderRepository) {
        _fileRepository = fileRepository;
        _folderRepository = folderRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index() {
        IResult<Folder[]> getRootFoldersResult = await _folderRepository.GetAllFromRootAsync();
        if (!getRootFoldersResult.Ok) {
            return StatusCode(StatusCodes.Status500InternalServerError, getRootFoldersResult.Reason);
        }

        Folder? desktopFolder = getRootFoldersResult.Value.FirstOrDefault(
            f => string.Equals(f.Name, "Desktop", StringComparison.CurrentCultureIgnoreCase)
        );
        if (desktopFolder == null) {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid ownerId)) {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Unable to get local user's ID. Please try again later."
                );
            }
            // Create it then!
            desktopFolder = new Folder {
                Name = "Desktop",
                OwnerId = ownerId
            };

            // Attempt to create the folder.
            IResult<Folder> createDesktopFolderResult = await _folderRepository.CreateAsync(desktopFolder);
            if (!createDesktopFolderResult.Ok) {
                return StatusCode(StatusCodes.Status500InternalServerError, createDesktopFolderResult.Reason);
            }

            desktopFolder = createDesktopFolderResult.Value;
        }

        // Get files from the "Desktop" folder.
        DesktopContent content = new () {
            Files = desktopFolder.SubFiles.ToArray(),
            Folders = desktopFolder.SubFolders.ToArray()
        };
        return View(content);
    }

    [HttpGet("Error/{statusCode:alpha}")]
    [AllowAnonymous]
    public IActionResult Error(string statusCode) => View($"Error/{statusCode}");

}