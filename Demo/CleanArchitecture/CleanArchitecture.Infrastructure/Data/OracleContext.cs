using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data
{
    internal class OracleContext(DbContextOptionsBuilder optionsBuilder) : ApplicationDbContext(optionsBuilder)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
                modelBuilder.HasSequence<long>("AccountNumbers");
                modelBuilder.Entity<AccountEntity>().Property(x => x.Sequence)
                .HasDefaultValueSql("NEXT VALUE FOR AccountNumbers");
                */
            base.OnModelCreating(modelBuilder);
        }
    }
}