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
        public DbSet<GuildMemberDonation> GuildMemberDonations { get; set; }

        public AldertoDbContext(DbContextOptions options) : base(options)
        {
        }

        public AldertoDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildMember>()
                .HasIndex(g => new { g.GuildId, g.MemberId })
                .IsUnique();

            // Guilds
            modelBuilder.Entity<Guild>()
                .HasOne(g => g.Configuration)
                .WithOne(c => c.Guild)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<GuildConfiguration>(configuration => configuration.GuildId);

            // Custom commands
            modelBuilder.Entity<CustomCommand>()
                .HasKey(m => new { m.GuildId, m.TriggerKeyword });

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
