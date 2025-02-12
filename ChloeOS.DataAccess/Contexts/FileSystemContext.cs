using ChloeOS.Core.Models;
using Microsoft.EntityFrameworkCore;
using File = ChloeOS.Core.Models.File;

namespace ChloeOS.DataAccess.Contexts;

public class FileSystemContext : DbContext {

    public DbSet<File> Files { get; set; }
    public DbSet<FileMetadata> FileMetadata { get; set; }

    public FileSystemContext(DbContextOptions<FileSystemContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        builder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();

        base.ConfigureConventions(builder);
    }

}