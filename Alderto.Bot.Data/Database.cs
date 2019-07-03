using Alderto.Bot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Bot.Data
{
    public class Database : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=store.db");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasMany(m => m.MembersRecruited)
                .WithOne(m => m.RecruitedByMember);

            base.OnModelCreating(modelBuilder);
        }
    }
}
