using System;
using System.Collections.Generic;
using System.Text;
using Alderto.Data;
using Alderto.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Alderto.Tests.MockedEntities
{
    class MockDbContext : DbContext, IAldertoDbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestDb");
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => base.OnModelCreating(modelBuilder);
    }
}
