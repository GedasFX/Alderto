using Alderto.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data
{
    public class AldertoDbContext : DbContext
    {
        public DbSet<Guild> Guilds => Set<Guild>();
        public DbSet<GuildMember> GuildMembers => Set<GuildMember>();
        public DbSet<Member> Members => Set<Member>();

        public DbSet<CustomCommand> CustomCommands => Set<CustomCommand>();

        public DbSet<GuildConfiguration> GuildPreferences => Set<GuildConfiguration>();
        public DbSet<GuildCommandAlias> GuildCommandAliases => Set<GuildCommandAlias>();

        public DbSet<GuildBank> GuildBanks => Set<GuildBank>();
        public DbSet<GuildBankItem> GuildBankItems => Set<GuildBankItem>();

        public DbSet<GuildManagedMessage> GuildManagedMessages => Set<GuildManagedMessage>();

        public DbSet<Currency> Currencies => Set<Currency>();
        public DbSet<CurrencyTransaction> CurrencyTransactions => Set<CurrencyTransaction>();
        public DbSet<GuildMemberWallet> GuildMemberWallets => Set<GuildMemberWallet>();

        public AldertoDbContext(DbContextOptions<AldertoDbContext> options) : base(options)
        {
        }

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

            modelBuilder.Entity<Currency>()
                .HasIndex(c => new { c.GuildId, c.Name })
                .IsUnique();

            modelBuilder.Entity<GuildManagedMessage>()
                .HasKey(m => new { m.GuildId, m.Id });

            modelBuilder.Entity<GuildCommandAlias>()
                .HasKey(c => new { c.GuildId, c.Alias });

            modelBuilder.Entity<CustomCommand>()
                .HasKey(m => new { m.GuildId, m.TriggerKeyword });

            base.OnModelCreating(modelBuilder);
        }
    }
}
