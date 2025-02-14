using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.Core.Models.FS;
using ChloeOS.DataAccess.Contexts;
using Jane;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.DataAccess.Repositories.FS;

public class FolderRepository : IFolderRepository {

    private readonly FileSystemContext _fs;

    public FolderRepository(FileSystemContext fs) => _fs = fs;

    public async Task<IResult<Folder[]>> GetAllFromParentAsync(Guid parentFolderId) {
        try {
            Folder[] folders = await _fs.Folders
                .Where(f => f.ParentFolderId == parentFolderId)
                .ToArrayAsync();

            return Result.Success(folders);
        } catch {
            return Result.Failure<Folder[]>(
                "Unable to retrieve folders within the current folder. Please try again later."
            );
        }
    }

    public async Task<IResult<Folder>> GetByIdAsync(Guid folderId) {
        try {
            Folder? folder = await _fs.Folders.FirstOrDefaultAsync(f => f.Id == folderId);

            if (folder == null) {
                return Result.Failure<Folder>("The requested folder was not found.");
            }

            return Result.Success(folder);
        } catch {
            return Result.Failure<Folder>("Unable to retrieve requested folder. Please try again later.");
        }
    }

    public async Task<IResult<Folder>> GetFromSubFileAsync(Guid subFileId) {
        try {
            Folder? folder = await _fs.Folders.FirstOrDefaultAsync(f => f.SubFiles.Any(fi => fi.Id == subFileId));

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
        await _fs.Folders.AddAsync(folder);

        int savedCount = await _fs.SaveChangesAsync();
        if (savedCount == 0) {
            return Result.Failure<Folder>("Unable to create this folder at the moment. Please try again later.");
        }

        return Result.Success(folder);
    }

    public async Task<IResult<Folder>> UpdateAsync(Folder folder) {
        _fs.Folders.Update(folder);

        int savedCount = await _fs.SaveChangesAsync();
        if (savedCount == 0) {
            return Result.Failure<Folder>("Unable to update this folder at the moment. Please try again later.");
        }

        return Result.Success(folder);
    }

    public async Task<IResult<Folder>> DeleteAsync(Folder folder) {
        _fs.Folders.Remove(folder);

        int savedCount = await _fs.SaveChangesAsync();
        if (savedCount == 0) {
            return Result.Failure<Folder>("Unable to delete this folder at the moment. Please try again later.");
        }

        return Result.Success(folder);
    }

}