using Alderto.Bot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Data
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }

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
                .HasIndex(m => new { m.MemberId, m.GuildId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
