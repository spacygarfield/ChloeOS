using Ardalis.Result;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.Core.Contracts.DataAccess.OS;

public interface IFileRepository {

    Task<Result<File[]>> GetAllFromRootAsync();
    Task<Result<File[]>> GetAllFromDirectoryAsync(Guid? directoryId);
    Task<Result<File>> GetByIdAsync(Guid fileId);
    Task<Result<File[]>> GetByNameAsync(string fileName, Guid? directoryId = null);
    Task<Result<File>> CreateAsync(File file);
    Task<Result<File>> UpdateAsync(File file);
    Task<Result> DeleteAsync(Guid fileId);

}