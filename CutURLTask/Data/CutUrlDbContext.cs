using CutURLTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace CutURLTask.Data
{
    public class CutUrlDbContext : DbContext
    {
        public CutUrlDbContext(DbContextOptions<CutUrlDbContext> options) : base(options) { }

        public DbSet<UrlRecord> UrlRecords { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UrlRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LongUrl)
                    .IsRequired();
                entity.Property(e => e.Code)
                    .HasMaxLength(8)
                    .IsRequired();
                entity.Property(e => e.UsedCount)
                    .HasDefaultValue(0);

                entity.HasIndex(e => e.LongUrl)
                    .IsUnique();

                entity.HasIndex(e => e.Code)
                    .IsUnique();

            });
        }
    }
}
