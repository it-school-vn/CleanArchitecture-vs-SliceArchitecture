using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Models.Account;
using CleanArchitecture.Domain.Models.Event;
using CleanArchitecture.Domain.Models.FeatureFlag;

using CleanArchitecture.Infrastructure.Converters;


namespace CleanArchitecture.Infrastructure.Data;

public abstract class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptionsBuilder optionsBuilder) :
    base(optionsBuilder.Options)
    {
        ChangeTracker.StateChanged += UpdateTimestamps!;
        ChangeTracker.Tracked += UpdateTimestamps!;

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        const string ownerKey = "OwnerId";
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AccountEntity>()
        .HasIndex(x => x.Email)
        .IsUnique()
        .HasFilter(null);

        modelBuilder.Entity<AccountEntity>()
               .OwnsMany(x => x.EducationInfos, a =>
               {
                   a.WithOwner().HasForeignKey(ownerKey);
                   a.ToJson();
               });

        modelBuilder.Entity<AccountEntity>().HasQueryFilter(x => x.Approval.Yes);


        modelBuilder.Entity<EventEntity>().HasQueryFilter(x => x.Approval.Yes);

        modelBuilder.Entity<EventEntity>().OwnsMany(x => x.Tags, a =>
        {
            a.WithOwner().HasForeignKey(ownerKey);
            a.ToTable("EventTags");
        });

        modelBuilder.Entity<EventAttendeeEntity>()
        .HasQueryFilter(x => x.Account != null && x.Account.Approval.Yes);


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
    public DbSet<AccountEntity> Accounts { get; set; }

    public DbSet<EventEntity> Events { get; set; }

    public DbSet<EventAttendeeEntity> EventAttendees { get; set; }

    public DbSet<FeatureFlagEntity> FeatureFlags { get; set; }

}