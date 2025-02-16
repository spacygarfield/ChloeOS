using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.DataAccess.Contexts;
using Jane;
using Microsoft.EntityFrameworkCore;
using Directory = ChloeOS.Core.Models.FS.Directory;

namespace ChloeOS.DataAccess.Repositories.FS;

public class DirectoryRepository : IDirectoryRepository {

    private readonly FileSystemContext _fs;

    public DirectoryRepository(FileSystemContext fs) => _fs = fs;

    public async Task<IResult<Directory[]>> GetAllFromRootAsync() {
        try {
            Directory[] folders = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .Where(f => !f.ParentId.HasValue)
                .ToArrayAsync();

            return Result.Success(folders);
        } catch {
            return Result.Failure<Directory[]>(
                "Unable to retrieve folders from the root. Please try again later."
            );
        }
    }

    public async Task<IResult<Directory[]>> GetAllFromParentAsync(Guid parentFolderId) {
        try {
            Directory[] folders = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .Where(f => f.ParentId == parentFolderId)
                .ToArrayAsync();

            if (folders.Length == 0) {
                return Result.Failure<Directory[]>("This folder contains no sub folders.");
            }

            return Result.Success(folders);
        } catch {
            return Result.Failure<Directory[]>(
                "Unable to retrieve folders within the current folder. Please try again later."
            );
        }
    }

    public async Task<IResult<Directory>> GetByIdAsync(Guid folderId) {
        try {
            Directory? folder = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.Id == folderId);

            if (folder == null) {
                return Result.Failure<Directory>("The requested folder was not found.");
            }

            return Result.Success(folder);
        } catch {
            return Result.Failure<Directory>("Unable to retrieve the requested folder. Please try again later.");
        }
    }

    public async Task<IResult<Directory>> GetFromSubFileAsync(Guid subFileId) {
        try {
            Directory? folder = await _fs.Directories
                .Include(f => f.SubFiles)
                .Include(f => f.SubDirectories)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.SubFiles.Any(sf => sf.Id == subFileId));

            if (folder == null) {
                return Result.Failure<Directory>("The folder containing the requested sub-file was not found.");
            }

            return Result.Success(folder);
        } catch {
            return Result.Failure<Directory>(
                "Unable to retrieve folder containing the requested sub-file. Please try again later."
            );
        }
    }

    public async Task<IResult<Directory>> CreateAsync(Directory directory) {
        try {
            await _fs.Directories.AddAsync(directory);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success(directory);
        } catch {
            return Result.Failure<Directory>("Unable to create this folder at the moment. Please try again later.");
        }
    }

    public async Task<IResult<Directory>> UpdateAsync(Directory directory) {
        try {
            _fs.Directories.Update(directory);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success(directory);
        } catch {
            return Result.Failure<Directory>("Unable to update this folder at the moment. Please try again later.");
        }
    }

    public async Task<IResult> DeleteAsync(Guid folderId) {
        try {
            int savedCount = await _fs.Directories
                .Where(f => f.Id == folderId)
                .ExecuteDeleteAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success();
        } catch {
            return Result.Failure("Unable to delete this folder at the moment. Please try again later.");
        }
    }

}