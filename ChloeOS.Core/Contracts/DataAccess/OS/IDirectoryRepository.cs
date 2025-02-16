using Jane;
using Directory = ChloeOS.Core.Models.FS.Directory;

namespace ChloeOS.Core.Contracts.DataAccess.OS;

public interface IDirectoryRepository {

    Task<IResult<Directory[]>> GetAllFromRootAsync();
    Task<IResult<Directory[]>> GetAllFromParentAsync(Guid parentFolderId);
    Task<IResult<Directory>> GetByIdAsync(Guid folderId);
    Task<IResult<Directory>> GetFromSubFileAsync(Guid subFileId);
    Task<IResult<Directory>> CreateAsync(Directory directory);
    Task<IResult<Directory>> UpdateAsync(Directory directory);
    Task<IResult> DeleteAsync(Guid folderId);

}