using ChloeOS.Business.Models;
using Microsoft.EntityFrameworkCore;
using File = ChloeOS.Business.Models.File;

namespace ChloeOS.DataAccess.Contexts;

public class FileSystemContext : DbContext {

    public DbSet<File> Files { get; set; }
    public DbSet<FileMetadata> FileMetadata { get; set; }

}