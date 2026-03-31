using EStudy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.DataAccess;

public class EStudyDbContext : DbContext
{
    public EStudyDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<Event> Events { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EStudyDbContext).Assembly);
    }
}