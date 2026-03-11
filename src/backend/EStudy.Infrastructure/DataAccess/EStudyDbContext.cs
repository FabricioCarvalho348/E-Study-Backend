using EStudy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EStudy.Infrastructure.DataAccess;

public class EStudyDbContext : DbContext
{
    public EStudyDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EStudyDbContext).Assembly);
    }
}