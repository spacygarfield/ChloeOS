using ChloeOS.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ChloeOS.Client.Database;

public class OperatingSystemContext : DbContext {

    public DbSet<User> Users { get; set; }

}