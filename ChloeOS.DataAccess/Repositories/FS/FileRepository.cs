using Ardalis.Result;
using ChloeOS.Core.Contracts.DataAccess.OS;
using ChloeOS.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.DataAccess.Repositories.FS;

public class FileRepository : IFileRepository {

    private readonly FileSystemContext _fs;

    public FileRepository(FileSystemContext fs) => _fs = fs;

    public async Task<Result<File[]>> GetAllFromRootAsync() {
        try {
            File[] files = await _fs.Files
                .Where(f => !f.DirectoryId.HasValue)
                .ToArrayAsync();

            return Result.Success(files);
        } catch {
            return Result.Unavailable("Unable to retrieve folders from the root. Please try again later.");
        }
    }

    public async Task<Result<File[]>> GetAllFromDirectoryAsync(Guid? parentDirectoryId) {
        try {
            File[] files = await _fs.Files
                .Where(f => f.DirectoryId == parentDirectoryId)
                .ToArrayAsync();

            if (files.Length == 0) {
                return Result.NotFound("This folder contains no files.");
            }

            return Result.Success(files);
        } catch {
            return Result.Unavailable("Unable to retrieve files within the requested folder. Please try again later.");
        }
    }

    public async Task<Result<File>> GetByIdAsync(Guid fileId) {
        try {
            File? file = await _fs.Files
                .Include(f => f.Parent)
                .FirstOrDefaultAsync(f => f.Id == fileId);

            if (file == null) {
                return Result.NotFound("The requested file was not found.");
            }

            return Result.Success(file);
        } catch {
            return Result.Unavailable("Unable to retrieve the requested file. Please try again later.");
        }
    }

    public async Task<Result<File[]>> GetByNameAsync(string fileName, Guid? directoryId = null) {
        try {
            File[] files = await _fs.Files
                .Include(f => f.Parent)
                .Where(f => EF.Functions.Like(f.Name, fileName) && f.DirectoryId == directoryId)
                .ToArrayAsync();

            if (files.Length == 0) {
                return Result.NotFound($"""No files match the name "{fileName}".""");
            }

            return Result.Success(files);
        } catch {
            return Result.Unavailable("Unable to retrieve the requested file. Please try again later.");
        }
    }

    public async Task<Result<File>> CreateAsync(File file) {
        try {
            await _fs.Files.AddAsync(file);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }
        } catch {
            return Result.Error("Unable to create this file at the moment. Please try again later.");
        }

        return Result.Success(file);
    }

    public async Task<Result<File>> UpdateAsync(File file) {
        try {
            _fs.Files.Update(file);

            int savedCount = await _fs.SaveChangesAsync();
            if (savedCount == 0) {
                throw new Exception();
            }
        } catch {
            return Result.Error("Unable to update this file at the moment. Please try again later.");
        }

        return Result.Success(file);
    }

    public async Task<Result> DeleteAsync(Guid fileId) {
        try {
            int savedCount = await _fs.Files
                .Where(f => f.Id == fileId)
                .ExecuteDeleteAsync();
            if (savedCount == 0) {
                throw new Exception();
            }
        } catch {
            return Result.Error("Unable to delete this file at the moment. Please try again later.");
        }

        return Result.Success();
    }

}