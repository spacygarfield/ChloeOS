using ChloeOS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using File = ChloeOS.Domain.Models.File;

namespace ChloeOS.DataAccess.Contexts;

public class FileSystemContext : DbContext {

    public DbSet<File> Files { get; set; }
    public DbSet<FileMetadata> FileMetadata { get; set; }

    public FileSystemContext(DbContextOptions<FileSystemContext> options) : base(options) { }

}