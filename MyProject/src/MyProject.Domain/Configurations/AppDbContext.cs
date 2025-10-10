using Microsoft.EntityFrameworkCore;

namespace MyProject.Domain.Configurations;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.HasPostgresExtension("uuid");
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}