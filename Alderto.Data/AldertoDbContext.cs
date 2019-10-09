using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data
{
    public class AldertoDbContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<CustomCommand> CustomCommands { get; set; }

        public DbSet<GuildConfiguration> GuildPreferences { get; set; }

        public DbSet<GuildBank> GuildBanks { get; set; }
        public DbSet<GuildBankItem> GuildBankItems { get; set; }

        public DbSet<GuildManagedMessage> GuildManagedMessages { get; set; }

#nullable disable
        public AldertoDbContext() { }
        public AldertoDbContext(DbContextOptions options) : base(options) { }
#nullable restore

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>()
                .HasOne(g => g.Configuration)
                .WithOne(c => c!.Guild!)
                .HasForeignKey<GuildConfiguration>(c => c.GuildId);

            modelBuilder.Entity<GuildMember>()
                .HasKey(g => new { g.GuildId, g.MemberId });

            modelBuilder.Entity<GuildBank>()
                // Frequent searches for guild banks are done by GuildId
                // Additionally a constraint for no duplicate names require unique index.
                .HasIndex(b => new { b.GuildId, b.Name })
                .IsUnique();

            modelBuilder.Entity<GuildManagedMessage>()
                .HasKey(m => new { m.GuildId, m.MessageId });

            modelBuilder.Entity<CustomCommand>()
                .HasKey(m => new { m.GuildId, m.TriggerKeyword });

            base.OnModelCreating(modelBuilder);
        }
    }
}
