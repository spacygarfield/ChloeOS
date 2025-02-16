using ChloeOS.Core.Models.FS;
using Microsoft.EntityFrameworkCore;
using Directory = ChloeOS.Core.Models.FS.Directory;
using File = ChloeOS.Core.Models.FS.File;

namespace ChloeOS.DataAccess.Contexts;

public class FileSystemContext : DbContext {

    public DbSet<Directory> Directories { get; set; }
    public DbSet<File> Files { get; set; }

    public FileSystemContext(DbContextOptions<FileSystemContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.HasDefaultSchema("fs");

        base.OnModelCreating(builder);
    }

}