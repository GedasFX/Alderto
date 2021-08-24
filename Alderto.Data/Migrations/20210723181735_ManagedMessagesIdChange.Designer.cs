﻿// <auto-generated />
using System;
using Alderto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Alderto.Data.Migrations
{
    [DbContext(typeof(AldertoDbContext))]
    [Migration("20210723181735_ManagedMessagesIdChange")]
    partial class ManagedMessagesIdChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Alderto.Data.Models.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("TimelyAmount")
                        .HasColumnType("integer");

                    b.Property<bool>("TimelyEnabled")
                        .HasColumnType("boolean");

                    b.Property<int>("TimelyInterval")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GuildId", "Name")
                        .IsUnique();

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Alderto.Data.Models.CurrencyTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsAward")
                        .HasColumnType("boolean");

                    b.Property<decimal>("RecipientId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("SenderId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("CurrencyTransactions");
                });

            modelBuilder.Entity("Alderto.Data.Models.CustomCommand", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("TriggerKeyword")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("LuaCode")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.HasKey("GuildId", "TriggerKeyword");

                    b.ToTable("CustomCommands");
                });

            modelBuilder.Entity("Alderto.Data.Models.Guild", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset?>("PremiumUntil")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("LogChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("ModeratorRoleId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.HasIndex("GuildId", "Name")
                        .IsUnique();

                    b.ToTable("GuildBanks");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildBankItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(280)
                        .HasColumnType("character varying(280)");

                    b.Property<int>("GuildBankId")
                        .HasColumnType("integer");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(140)
                        .HasColumnType("character varying(140)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("character varying(70)");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("GuildBankId");

                    b.ToTable("GuildBankItems");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildCommandAlias", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Alias")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.HasKey("GuildId", "Alias");

                    b.ToTable("GuildCommandAliases");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildConfiguration", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("LogChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("ModeratorRoleId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Prefix")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("GuildId");

                    b.ToTable("GuildPreferences");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildManagedMessage", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ChannelId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Content")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("GuildId", "Id");

                    b.ToTable("GuildManagedMessages");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildMember", b =>
                {
                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MemberId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset?>("JoinedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Nickname")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<decimal?>("RecruiterMemberId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("GuildId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("GuildMembers");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildMemberWallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("MemberId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTimeOffset>("TimelyLastClaimed")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("MemberId");

                    b.ToTable("GuildMemberWallets");
                });

            modelBuilder.Entity("Alderto.Data.Models.Member", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Discriminator")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Username")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Alderto.Data.Models.Currency", b =>
                {
                    b.HasOne("Alderto.Data.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Alderto.Data.Models.CurrencyTransaction", b =>
                {
                    b.HasOne("Alderto.Data.Models.Currency", "Currency")
                        .WithMany("Transactions")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Alderto.Data.Models.Member", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Alderto.Data.Models.Member", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Alderto.Data.Models.CustomCommand", b =>
                {
                    b.HasOne("Alderto.Data.Models.Guild", "Guild")
                        .WithMany("CustomCommands")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildBank", b =>
                {
                    b.HasOne("Alderto.Data.Models.Guild", "Guild")
                        .WithMany("GuildBanks")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildBankItem", b =>
                {
                    b.HasOne("Alderto.Data.Models.GuildBank", "GuildBank")
                        .WithMany("Contents")
                        .HasForeignKey("GuildBankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GuildBank");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildCommandAlias", b =>
                {
                    b.HasOne("Alderto.Data.Models.Guild", "Guild")
                        .WithMany("Aliases")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildConfiguration", b =>
                {
                    b.HasOne("Alderto.Data.Models.Guild", "Guild")
                        .WithOne("Configuration")
                        .HasForeignKey("Alderto.Data.Models.GuildConfiguration", "GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildManagedMessage", b =>
                {
                    b.HasOne("Alderto.Data.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildMember", b =>
                {
                    b.HasOne("Alderto.Data.Models.Guild", "Guild")
                        .WithMany("GuildMembers")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Alderto.Data.Models.Member", "Member")
                        .WithMany("GuildMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildMemberWallet", b =>
                {
                    b.HasOne("Alderto.Data.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Alderto.Data.Models.Member", "Member")
                        .WithMany("Wallets")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Alderto.Data.Models.Currency", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Alderto.Data.Models.Guild", b =>
                {
                    b.Navigation("Aliases");

                    b.Navigation("Configuration");

                    b.Navigation("CustomCommands");

                    b.Navigation("GuildBanks");

                    b.Navigation("GuildMembers");
                });

            modelBuilder.Entity("Alderto.Data.Models.GuildBank", b =>
                {
                    b.Navigation("Contents");
                });

            modelBuilder.Entity("Alderto.Data.Models.Member", b =>
                {
                    b.Navigation("GuildMembers");

                    b.Navigation("Wallets");
                });
#pragma warning restore 612, 618
        }
    }
}
