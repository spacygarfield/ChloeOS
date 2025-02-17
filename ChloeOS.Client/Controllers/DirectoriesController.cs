using Ardalis.Result;
using ChloeOS.Core.Contracts.DataAccess.OS;
using Microsoft.AspNetCore.Mvc;
using Directory = ChloeOS.Core.Models.FS.Directory;

namespace ChloeOS.Client.Controllers;

[Route("fs/dir")]
public class DirectoriesController : Controller {

    private readonly IDirectoryRepository _directoryRepository;

    public DirectoriesController(IDirectoryRepository directoryRepository)
        => _directoryRepository = directoryRepository;

    [HttpGet("id/{directoryId:guid}")]
    public async Task<IActionResult> DirectoryById([FromRoute] Guid directoryId) {
        Result<Directory> getDirectoryResult = await _directoryRepository.GetByIdAsync(directoryId);
        if (getDirectoryResult.IsUnavailable()) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, getDirectoryResult.Errors);
        }
        if (getDirectoryResult.IsNotFound()) {
            return NotFound(getDirectoryResult.Errors);
        }

        return Ok(getDirectoryResult.Value);
    }

    [HttpGet("name/{directoryName:alpha}")]
    public async Task<IActionResult> DirectoryName([FromRoute] string directoryName, [FromQuery] Guid? parentId) {
        Result<Directory[]> getDirectoriesResult = await _directoryRepository.GetByNameAsync(directoryName);
        if (getDirectoriesResult.IsUnavailable()) {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, getDirectoriesResult.Errors);
        }
        if (getDirectoriesResult.IsNotFound()) {
            return NotFound(getDirectoriesResult.Errors);
        }

        return Ok(getDirectoriesResult.Value);
    }

}