using ChloeOS.Core.Models;
using ChloeOS.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.Client.Database;

public class OperatingSystemContext : DbContext {

    public DbSet<User> Users { get; set; }

    public OperatingSystemContext(DbContextOptions<OperatingSystemContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder) {
        builder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();

        base.ConfigureConventions(builder);
    }

}