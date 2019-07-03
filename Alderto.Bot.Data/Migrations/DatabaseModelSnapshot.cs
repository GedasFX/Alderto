﻿// <auto-generated />
using System;
using Alderto.Bot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alderto.Bot.Data.Migrations
{
    [DbContext(typeof(SqliteDbContext))]
    partial class DatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("Alderto.Bot.Data.Models.Guild", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Alderto.Bot.Data.Models.Member", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<int>("CurrencyCount");

                    b.Property<DateTime?>("CurrencyLastClaimed");

                    b.Property<string>("GuildId");

                    b.Property<string>("MemberId");

                    b.Property<string>("RecruitedByMemberId");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Alderto.Bot.Data.Models.Member", b =>
                {
                    b.HasOne("Alderto.Bot.Data.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId");

                    b.HasOne("Alderto.Bot.Data.Models.Member", "RecruitedByMember")
                        .WithMany("MembersRecruited")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
