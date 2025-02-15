using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.FS;
using ChloeOS.DataAccess.Contexts;
using Jane;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.DataAccess.Repositories.FS;

public class FolderRepository : IFolderRepository {

    private readonly FileSystemContext _fs;

    public FolderRepository(FileSystemContext fs) => _fs = fs;

    public async Task<IResult<Folder[]>> GetAllFromRootAsync() {
        try {
            Folder[] folders = await _fs.Folders
                .Include(f => f.SubFiles)
                .Include(f => f.SubFolders)
                // .AsSplitQuery()
                .Where(f => !f.ParentId.HasValue)
                .ToArrayAsync();

            return Result.Success(folders);
        } catch {
            return Result.Failure<Folder[]>(
                "Unable to retrieve folders from the root. Please try again later."
            );
        }
    }

    public async Task<IResult<Folder[]>> GetAllFromParentAsync(Guid parentFolderId) {
        try {
            Folder[] folders = await _fs.Folders
                .Include(f => f.SubFiles)
                .Include(f => f.SubFolders)
                // .AsSplitQuery()
                .Where(f => f.ParentId == parentFolderId)
                .ToArrayAsync();

            if (folders.Length == 0) {
                return Result.Failure<Folder[]>("This folder contains no sub folders.");
            }

            return Result.Success(folders);
        } catch {
            return Result.Failure<Folder[]>(
                "Unable to retrieve folders within the current folder. Please try again later."
            );
        }
    }

    public async Task<IResult<Folder>> GetByIdAsync(Guid folderId) {
        try {
            Folder? folder = await _fs.Folders
                .Include(f => f.SubFiles)
                .Include(f => f.SubFolders)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.Id == folderId);

            if (folder == null) {
                return Result.Failure<Folder>("The requested folder was not found.");
            }

            return Result.Success(folder);
        } catch {
            return Result.Failure<Folder>("Unable to retrieve the requested folder. Please try again later.");
        }
    }

    public async Task<IResult<Folder>> GetFromSubFileAsync(Guid subFileId) {
        try {
            Folder? folder = await _fs.Folders
                .Include(f => f.SubFiles)
                .Include(f => f.SubFolders)
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.SubFiles.Any(sf => sf.Id == subFileId));

            if (folder == null) {
                return Result.Failure<Folder>("The folder containing the requested sub-file was not found.");
            }

            return Result.Success(folder);
        } catch {
            return Result.Failure<Folder>(
                "Unable to retrieve folder containing the requested sub-file. Please try again later."
            );
        }
    }

    public async Task<IResult<Folder>> CreateAsync(Folder folder) {
        try {
            await _fs.Folders.AddAsync(folder);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success(folder);
        } catch {
            return Result.Failure<Folder>("Unable to create this folder at the moment. Please try again later.");
        }
    }

    public async Task<IResult<Folder>> UpdateAsync(Folder folder) {
        try {
            _fs.Folders.Update(folder);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }

            return Result.Success(folder);
        } catch {
            return Result.Failure<Folder>("Unable to update this folder at the moment. Please try again later.");
        }
    }

    public async Task<IResult> DeleteAsync(Guid folderId) {
        try {
            int savedCount = await _fs.Folders
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