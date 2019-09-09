using Alderto.Data.Models;
using Alderto.Data.Models.GuildBank;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Data
{
    public class AldertoDbContext : DbContext, IAldertoDbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<CustomCommand> CustomCommands { get; set; }

        public DbSet<GuildConfiguration> GuildPreferences { get; set; }

        public DbSet<GuildBank> GuildBanks { get; set; }
        public DbSet<GuildBankItem> GuildBankItems { get; set; }

        public AldertoDbContext(DbContextOptions options) : base(options)
        {
        }

        public AldertoDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>()
                .HasOne(g => g.Configuration)
                .WithOne(c => c.Guild)
                .HasForeignKey<GuildConfiguration>(c => c.GuildId);

            modelBuilder.Entity<GuildMember>()
                .HasKey(g => new { g.GuildId, g.MemberId });

            modelBuilder.Entity<GuildBank>()
                .HasIndex(b => new { b.GuildId, b.Name })
                .IsUnique();
            
            modelBuilder.Entity<CustomCommand>()
                .HasKey(m => new { m.GuildId, m.TriggerKeyword });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//#if DEBUG
//            optionsBuilder.EnableSensitiveDataLogging();
//            optionsBuilder.UseSqlServer(
//                "Server=(localdb)\\mssqllocaldb;Database=Alderto;Trusted_Connection=True;MultipleActiveResultSets=true");
//#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
