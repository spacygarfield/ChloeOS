using ChloeOS.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.DataAccess.Contexts;

public class OperatingSystemContext : DbContext {

    public DbSet<User> Users { get; set; }

    public OperatingSystemContext(DbContextOptions<OperatingSystemContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.HasDefaultSchema("os");

        base.OnModelCreating(builder);
    }

}