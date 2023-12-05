using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data
{
    internal class MariaDbContext(DbContextOptionsBuilder optionsBuilder) : ApplicationDbContext(optionsBuilder)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}