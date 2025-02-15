using ChloeOS.Core.Models.FS;
using Jane;

namespace ChloeOS.Core.Contracts.DataAccess.OS;

public interface IFolderRepository {

    Task<IResult<Folder[]>> GetAllFromRootAsync();
    Task<IResult<Folder[]>> GetAllFromParentAsync(Guid parentFolderId);
    Task<IResult<Folder>> GetByIdAsync(Guid folderId);
    Task<IResult<Folder>> GetFromSubFileAsync(Guid subFileId);
    Task<IResult<Folder>> CreateAsync(Folder folder);
    Task<IResult<Folder>> UpdateAsync(Folder folder);
    Task<IResult> DeleteAsync(Guid folderId);

}