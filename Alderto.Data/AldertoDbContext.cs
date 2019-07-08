using Alderto.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data
{
    public class AldertoDbContext : IdentityDbContext<IdentityUser>, IAldertoDbContext
    {
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<CustomCommand> CustomCommands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildMember>()
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=Alderto;Trusted_Connection=True;MultipleActiveResultSets=true");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
