using ChloeOS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.Client.Database;

public class OperatingSystemContext : DbContext {

    public DbSet<User> Users { get; set; }

    public OperatingSystemContext(DbContextOptions<OperatingSystemContext> options) : base(options) { }

}