using Microsoft.EntityFrameworkCore;
using OnionDemo.Domain.Entities;

namespace OnionDemo.Persistance
{
    public sealed class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<PingPong> PingPong { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
    }
}
