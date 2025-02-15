using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.DataAccess.Contexts;
using Jane;
using Microsoft.EntityFrameworkCore;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.DataAccess.Repositories.FS;

public class FileRepository : IFileRepository {

    private readonly FileSystemContext _fs;

    public FileRepository(FileSystemContext fs) => _fs = fs;

    public async Task<IResult<File[]>> GetAllFromRootAsync() {
        try {
            File[] files = await _fs.Files
                .Where(f => !f.FolderId.HasValue)
                .ToArrayAsync();

            return Result.Success(files);
        } catch {
            return Result.Failure<File[]>(
                "Unable to retrieve folders from the root. Please try again later."
            );
        }
    }

    public async Task<IResult<File[]>> GetAllFromFolderAsync(Guid? parentFolderId) {
        try {
            File[] files = await _fs.Files
                .Where(f => f.FolderId == parentFolderId)
                .ToArrayAsync();

            if (files.Length == 0) {
                return Result.Failure<File[]>("This folder contains no files.");
            }

            return Result.Success(files);
        } catch {
            return Result.Failure<File[]>(
                "Unable to retrieve files within the requested folder. Please try again later."
            );
        }
    }

    public async Task<IResult<File>> GetByIdAsync(Guid fileId) {
        try {
            File? file = await _fs.Files.FindAsync(fileId);

            if (file == null) {
                throw new Exception();
            }

            return Result.Success(file);
        } catch {
            return Result.Failure<File>("Unable to retrieve the requested file. Please try again later.");
        }
    }

    public async Task<IResult<File>> CreateAsync(File file) {
        try {
            await _fs.Files.AddAsync(file);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }
        } catch {
            return Result.Failure<File>("Unable to create this file at the moment. Please try again later.");
        }

        return Result.Success(file);
    }

    public async Task<IResult<File>> UpdateAsync(File file) {
        try {
            _fs.Files.Update(file);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }
        } catch {
            return Result.Failure<File>("Unable to update this file at the moment. Please try again later.");
        }

        return Result.Success(file);
    }

    public async Task<IResult> DeleteAsync(Guid fileId) {
        try {
            int savedCount = await _fs.Files
                .Where(f => f.Id == fileId)
                .ExecuteDeleteAsync();
            if (savedCount == 0) {
                throw new Exception();
            }
        } catch {
            return Result.Failure("Unable to delete this file at the moment. Please try again later.");
        }

        return Result.Success();
    }

}