using Alderto.Data;
using Alderto.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Tests.MockedEntities
{
    internal class MockDbContext : DbContext, IAldertoDbContext
    {
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<CustomCommand> CustomCommands { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<GuildConfiguration> GuildPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildMember>()
                .HasKey(m => new { m.MemberId, m.GuildId });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestDb");
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
