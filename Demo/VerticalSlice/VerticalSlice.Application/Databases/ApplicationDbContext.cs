using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VerticalSlice.Application.Converters;
using VerticalSlice.Application.Entities;



namespace VerticalSlice.Application.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptionsBuilder optionsBuilder) :
    base(optionsBuilder.Options)
    {
        ChangeTracker.StateChanged += UpdateTimestamps!;
        ChangeTracker.Tracked += UpdateTimestamps!;

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

    }

    private static void UpdateTimestamps(object sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is IHasTimestamp entityWithTimestamps)
        {
            switch (e.Entry.State)
            {
                case EntityState.Deleted:
                    break;

                case EntityState.Modified:
                    entityWithTimestamps.Modified = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Added:
                    entityWithTimestamps.Added = DateTimeOffset.UtcNow;
                    break;
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();

    }
    public DbSet<TaskEntity> Tasks { get; set; }


}