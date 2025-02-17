using Ardalis.Result;
using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Directory = ChloeOS.Core.Models.FS.Directory;

namespace ChloeOS.DataAccess.Repositories.FS;

public class DirectoryRepository : IDirectoryRepository {

    private readonly FileSystemContext _fs;

    public DirectoryRepository(FileSystemContext fs) => _fs = fs;

    public async Task<Result<Directory[]>> GetAllFromRootAsync() {
        try {
            Directory[] directories = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .Where(f => !f.ParentId.HasValue)
                .ToArrayAsync();

            return Result.Success(directories);
        } catch {
            return Result.Unavailable("Unable to retrieve folders from the root. Please try again later.");
        }
    }

    public async Task<Result<Directory[]>> GetAllFromParentAsync(Guid parentDirectoryId) {
        try {
            Directory[] directories = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .Where(f => f.ParentId == parentDirectoryId)
                .ToArrayAsync();

            if (directories.Length == 0) {
                return Result.NotFound("This folder contains no sub folders.");
            }

            return Result.Success(directories);
        } catch {
            return Result.Unavailable("Unable to retrieve folders within the current folder. Please try again later.");
        }
    }

    public async Task<Result<Directory>> GetByIdAsync(Guid directoryId) {
        try {
            Directory? directory = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.Id == directoryId);

            if (directory == null) {
                return Result.NotFound("The requested folder was not found.");
            }

            return Result.Success(directory);
        } catch {
            return Result.Unavailable("Unable to retrieve the requested folder. Please try again later.");
        }
    }

    public async Task<Result<Directory[]>> GetByNameAsync(string directoryName, Guid? parentDirectoryId = null) {
        try {
            // Use the query and adjust it before retrieving all matching records from the database.
            Directory[] directories = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .Where(f => EF.Functions.Like(f.Name, directoryName) && f.ParentId == parentDirectoryId)
                .ToArrayAsync();

            if (directories.Length == 0) {
                return Result.NotFound($"""No folders match the name "{directoryName}".""");
            }

            return Result.Success(directories);
        } catch {
            return Result.Unavailable("Unable to retrieve the requested folder. Please try again later.");
        }
    }

    public async Task<Result<Directory>> GetFromSubFileAsync(Guid subFileId) {
        try {
            Directory? directory = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.SubFiles.Any(sf => sf.Id == subFileId));

            if (directory == null) {
                return Result.NotFound("The folder containing the requested sub-file was not found.");
            }

            return Result.Success(directory);
        } catch {
            return Result.Unavailable(
                "Unable to retrieve folder containing the requested sub-file. Please try again later."
            );
        }
    }

    public async Task<Result<Directory>> CreateAsync(Directory directory) {
        try {
            await _fs.Directories.AddAsync(directory);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success(directory);
        } catch {
            return Result.Error("Unable to create this folder at the moment. Please try again later.");
        }
    }

    public async Task<Result<Directory>> UpdateAsync(Directory directory) {
        try {
            _fs.Directories.Update(directory);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success(directory);
        } catch {
            return Result.Error("Unable to update this folder at the moment. Please try again later.");
        }
    }

    public async Task<Result> DeleteAsync(Guid directoryId) {
        try {
            int savedCount = await _fs.Directories
                .Where(f => f.Id == directoryId)
                .ExecuteDeleteAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success();
        } catch {
            return Result.Error("Unable to delete this folder at the moment. Please try again later.");
        }
    }

}