using Dev_Events_App.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dev_Events_App.Persistence
{
    public class DevEventsDbContext : DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {

        }

        public DbSet<DevEvent> DevEvents { get; set; }
        public DbSet<DevEventSpeaker> DevEventsSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevEvent>(e =>
            {
                e.HasKey(d => d.Id);

                e.Property(d => d.Title).IsRequired(false);

                e.Property(d => d.Description)
                    .HasMaxLength(200)
                    .HasColumnType("varchar(200)");

                e.Property(d => d.StartDate)
                    .HasColumnName("Start_Date");

                e.Property(d => d.EndDate)
                    .HasColumnName("End_Date");

                e.HasMany(d => d.Speakers)
                    .WithOne()
                    .HasForeignKey(s => s.DevEventId);
            });

            builder.Entity<DevEventSpeaker>(e =>
            {
                e.HasKey(d => d.DevEventId);
            });
        }
    }
}