using Alderto.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data
{
    public class AldertoDbContext : IdentityDbContext<ApplicationUser, IdentityRole<ulong>, ulong>, IAldertoDbContext
    {
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<CustomCommand> CustomCommands { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<GuildConfiguration> GuildPreferences { get; set; }

        public AldertoDbContext(DbContextOptions options) : base(options)
        {
        }

        public AldertoDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Guild Members
            modelBuilder.Entity<GuildMember>()
                .HasKey(m => new { m.MemberId, m.GuildId });

            // Guilds
            modelBuilder.Entity<Guild>()
                .HasMany(g => g.GuildMembers)
                .WithOne(m => m.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guild>()
                .HasMany(g => g.CustomCommands)
                .WithOne(cc => cc.Guild)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Guild>()
                .HasOne(g => g.Configuration)
                .WithOne(c => c.Guild)
                .HasForeignKey<GuildConfiguration>(configuration => configuration.Id);

            // Custom commands
            modelBuilder.Entity<CustomCommand>()
                .HasKey(m => new { m.GuildId, m.TriggerKeyword });

            // Members
            modelBuilder.Entity<Member>()
                .HasMany(m => m.GuildMembers)
                .WithOne(gm => gm.Member)
                .OnDelete(DeleteBehavior.Cascade);

            // Guild Preferences
            

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=Alderto;Trusted_Connection=True;MultipleActiveResultSets=true");
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
