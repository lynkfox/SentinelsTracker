﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using website.Models;

namespace website.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("website.Models.databaseModels.AccessChange", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessLevelId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AccessLevelId");

                    b.HasIndex("UserId");

                    b.ToTable("AccessChanges","logging");
                });

            modelBuilder.Entity("website.Models.databaseModels.AccessLevel", b =>
                {
                    b.Property<int>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LevelCommonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Modify")
                        .HasColumnType("bit");

                    b.Property<bool>("Read")
                        .HasColumnType("bit");

                    b.HasKey("Level");

                    b.ToTable("AccessLevels","users");
                });

            modelBuilder.Entity("website.Models.databaseModels.BoxSet", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("PublicationDate")
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("WikiLink")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.HasKey("ID");

                    b.ToTable("BoxSets","gamedata");
                });

            modelBuilder.Entity("website.Models.databaseModels.EnvironmentUsed", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Destroyed")
                        .HasColumnType("bit");

                    b.Property<int>("GameDetailId")
                        .HasColumnType("int");

                    b.Property<int>("GameEnvironmentId")
                        .HasColumnType("int");

                    b.Property<int>("OblivAeonOrderAppeared")
                        .HasColumnType("int");

                    b.Property<int>("OblivAeonZone")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("GameDetailId");

                    b.HasIndex("GameEnvironmentId");

                    b.ToTable("EnvironmentsUsed","statistics");
                });

            modelBuilder.Entity("website.Models.databaseModels.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateEntered")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("Games","statistics");
                });

            modelBuilder.Entity("website.Models.databaseModels.GameDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("CalculatedDifficulty")
                        .HasColumnType("float");

                    b.Property<string>("GameEndCondition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("GameMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameTimeLength")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseRule")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfPlayers")
                        .HasColumnType("int");

                    b.Property<string>("OtherSelections")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<int>("PerceivedDifficulty")
                        .HasColumnType("int");

                    b.Property<string>("Platform")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Rounds")
                        .HasColumnType("int");

                    b.Property<string>("SelectionMethod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserComment")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(5000);

                    b.Property<bool>("Win")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("GameDetails","statistics");
                });

            modelBuilder.Entity("website.Models.databaseModels.GameEnvironment", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<int>("BoxSetId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("WikiLink")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.HasKey("ID");

                    b.HasIndex("BoxSetId");

                    b.ToTable("GameEnvironments","gamedata");
                });

            modelBuilder.Entity("website.Models.databaseModels.Hero", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("BaseHero")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("BoxSetId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.Property<bool>("IsAlt")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("PrintedComplexity")
                        .HasColumnType("int");

                    b.Property<string>("Team")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("WikiLink")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.HasKey("ID");

                    b.HasIndex("BoxSetId");

                    b.ToTable("HeroCharacters","gamedata");
                });

            modelBuilder.Entity("website.Models.databaseModels.HeroTeam", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameDetailId")
                        .HasColumnType("int");

                    b.Property<int>("HeroId")
                        .HasColumnType("int");

                    b.Property<bool>("Incapped")
                        .HasColumnType("bit");

                    b.Property<int>("OblivAeonIncapOrder")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("GameDetailId");

                    b.HasIndex("HeroId");

                    b.ToTable("HeroTeams","statistics");
                });

            modelBuilder.Entity("website.Models.databaseModels.LoginAttempt", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AttemptTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("LoginAttempts","logging");
                });

            modelBuilder.Entity("website.Models.databaseModels.PasswordHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ChangedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PreviousHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousSalt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordHistories","logging");
                });

            modelBuilder.Entity("website.Models.databaseModels.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("HasClaimed")
                        .HasColumnType("bit");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<string>("Profile")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("UserIcon")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("ID");

                    b.ToTable("Users","users");
                });

            modelBuilder.Entity("website.Models.databaseModels.UserPermission", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessLevelId")
                        .HasColumnType("int");

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("AccessLevelId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserPermissions","users");
                });

            modelBuilder.Entity("website.Models.databaseModels.Villain", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.Property<string>("BaseName")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("BoxSetId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<int>("PrintedDifficulty")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WikiLink")
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.HasKey("ID");

                    b.HasIndex("BoxSetId");

                    b.ToTable("VillainCharacters","gamedata");
                });

            modelBuilder.Entity("website.Models.databaseModels.VillainTeam", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GameDetailID")
                        .HasColumnType("int");

                    b.Property<bool>("Incapped")
                        .HasColumnType("bit");

                    b.Property<bool>("OblivAeon")
                        .HasColumnType("bit");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("VillainId")
                        .HasColumnType("int");

                    b.Property<bool>("VillainTeamGame")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("GameDetailID");

                    b.HasIndex("VillainId");

                    b.ToTable("VillainTeams","statistics");
                });

            modelBuilder.Entity("website.Models.databaseModels.AccessChange", b =>
                {
                    b.HasOne("website.Models.databaseModels.AccessLevel", "AccessLevel")
                        .WithMany("AccessChanges")
                        .HasForeignKey("AccessLevelId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("website.Models.databaseModels.User", "User")
                        .WithMany("AccessChanges")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.EnvironmentUsed", b =>
                {
                    b.HasOne("website.Models.databaseModels.GameDetail", "GameDetail")
                        .WithMany("EnvironmentsUsed")
                        .HasForeignKey("GameDetailId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("website.Models.databaseModels.GameEnvironment", "GameEnvironment")
                        .WithMany("EnvironmentsUsed")
                        .HasForeignKey("GameEnvironmentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.Game", b =>
                {
                    b.HasOne("website.Models.databaseModels.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("website.Models.databaseModels.GameDetail", b =>
                {
                    b.HasOne("website.Models.databaseModels.Game", "Game")
                        .WithOne("GameDetail")
                        .HasForeignKey("website.Models.databaseModels.GameDetail", "GameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.GameEnvironment", b =>
                {
                    b.HasOne("website.Models.databaseModels.BoxSet", "BoxSet")
                        .WithMany("GameEnvironments")
                        .HasForeignKey("BoxSetId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.Hero", b =>
                {
                    b.HasOne("website.Models.databaseModels.BoxSet", "BoxSet")
                        .WithMany("Heroes")
                        .HasForeignKey("BoxSetId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.HeroTeam", b =>
                {
                    b.HasOne("website.Models.databaseModels.GameDetail", "GameDetail")
                        .WithMany("HeroTeams")
                        .HasForeignKey("GameDetailId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("website.Models.databaseModels.Hero", "Hero")
                        .WithMany("HeroTeams")
                        .HasForeignKey("HeroId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.LoginAttempt", b =>
                {
                    b.HasOne("website.Models.databaseModels.User", "User")
                        .WithMany("LoginAttempts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.PasswordHistory", b =>
                {
                    b.HasOne("website.Models.databaseModels.User", "User")
                        .WithMany("PasswordHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.UserPermission", b =>
                {
                    b.HasOne("website.Models.databaseModels.AccessLevel", "AccessLevel")
                        .WithMany("UserPermissions")
                        .HasForeignKey("AccessLevelId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("website.Models.databaseModels.User", "User")
                        .WithOne("UserPermission")
                        .HasForeignKey("website.Models.databaseModels.UserPermission", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.Villain", b =>
                {
                    b.HasOne("website.Models.databaseModels.BoxSet", "BoxSet")
                        .WithMany("Villains")
                        .HasForeignKey("BoxSetId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("website.Models.databaseModels.VillainTeam", b =>
                {
                    b.HasOne("website.Models.databaseModels.GameDetail", null)
                        .WithMany("VillainTeams")
                        .HasForeignKey("GameDetailID")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("website.Models.databaseModels.Villain", "Villain")
                        .WithMany("VillainTeams")
                        .HasForeignKey("VillainId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
