using Alderto.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data
{
    public class SqliteDbContext : DbContext, IAldertoDbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<CustomCommand> CustomCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=store.db");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasMany(m => m.MembersRecruited)
                .WithOne(m => m.RecruitedByMember)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Member>()
                .HasIndex(m => new { m.MemberId, m.GuildId })
                .IsUnique();

            modelBuilder.Entity<Guild>()
                .HasMany(g => g.Members)
                .WithOne(m => m.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guild>()
                .HasMany(g => g.CustomCommands)
                .WithOne(cc => cc.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomCommand>()
                .HasIndex(m => new { m.GuildId, m.TriggerKeyword })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
