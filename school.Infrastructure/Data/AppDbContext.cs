using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using school.Domain.Entities;

namespace school.Infrastructure.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Student>().ToTable("Students");
        modelBuilder.Entity<Teacher>().ToTable("Teachers");
        
        base.OnModelCreating(modelBuilder);
    }
}