using ChloeOS.Core.Models.FS;
using Microsoft.EntityFrameworkCore;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.DataAccess.Contexts;

public class FileSystemContext : DbContext {

    public DbSet<File> Files { get; set; }
    public DbSet<FileMetadata> FileMetadata { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<FolderMetadata> FolderMetadata { get; set; }

    public FileSystemContext(DbContextOptions<FileSystemContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.HasDefaultSchema("fs");

        base.OnModelCreating(builder);
    }

}