using Microsoft.EntityFrameworkCore;
using MinimalPlus.Models;

namespace MinimalPlus;

public class ApplicationDatabaseContext : DbContext
{
    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}