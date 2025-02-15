using Jane;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.Core.Contracts.DataAccess.OS;

public interface IFileRepository {

    Task<IResult<File[]>> GetAllFromRootAsync();
    Task<IResult<File[]>> GetAllFromFolderAsync(Guid? parentFolderId);
    Task<IResult<File>> GetByIdAsync(Guid fileId);
    Task<IResult<File>> CreateAsync(File file);
    Task<IResult<File>> UpdateAsync(File file);
    Task<IResult> DeleteAsync(Guid fileId);

}