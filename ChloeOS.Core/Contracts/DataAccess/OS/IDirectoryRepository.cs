using Ardalis.Result;
using Directory = ChloeOS.Core.Models.FS.Directory;

namespace ChloeOS.Core.Contracts.DataAccess.OS;

public interface IDirectoryRepository {

    Task<Result<Directory[]>> GetAllFromRootAsync();
    Task<Result<Directory[]>> GetAllFromParentAsync(Guid parentDirectoryId);
    Task<Result<Directory>> GetByIdAsync(Guid directoryId);
    Task<Result<Directory[]>> GetByNameAsync(string directoryName, Guid? parentDirectoryId = null);
    Task<Result<Directory>> GetFromSubFileAsync(Guid subFileId);
    Task<Result<Directory>> CreateAsync(Directory directory);
    Task<Result<Directory>> UpdateAsync(Directory directory);
    Task<Result> DeleteAsync(Guid directoryId);

}