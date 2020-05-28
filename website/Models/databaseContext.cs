using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using website.Models.databaseModels;

namespace website.Models
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(string connectionString) : base(GetOptions(connectionString))
        {

        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        private static DbContextOptions GetOptions(string cnn)
        {
            var modelBuilder = new DbContextOptionsBuilder();
            return modelBuilder.UseSqlServer(cnn).Options;

        }

        public DbSet<Game> Games { get; set; }
        public DbSet<GameDetail> GameDetails { get; set; }
        public DbSet<GameEnvironment> GameEnvironments { get; set; }
        public DbSet<HeroTeam> HeroeTeams { get; set; }
        public DbSet<VillainTeam> VillainTeams { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Villain> Villains { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<AccessLevel> AccessLevels { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }



        //Fluid API to clean up some things
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //these define the Enums in GameDetails 
            modelBuilder.Entity<GameDetail>()
                .Property(x => x.Platform)
                .HasConversion(x => x.ToString(), x => (GameDetail.Platforms)Enum.Parse(typeof(GameDetail.Platforms), x));

            modelBuilder.Entity<GameDetail>()
                .Property(x => x.SelectionMethod)
                .HasConversion(x => x.ToString(), x => (GameDetail.SelectionMethods)Enum.Parse(typeof(GameDetail.SelectionMethods), x));

            modelBuilder.Entity<GameDetail>()
                .Property(x => x.GameMode)
                .HasConversion(x => x.ToString(), x => (GameDetail.GameModes)Enum.Parse(typeof(GameDetail.GameModes), x));

            modelBuilder.Entity<GameDetail>()
                .Property(x => x.GameEndCondition)
                .HasConversion(x => x.ToString(), x => (GameDetail.GameEndConditions)Enum.Parse(typeof(GameDetail.GameEndConditions), x));

            //Annotations was being a brat so GameEnvironment FKey in GameDetails is defined here.
            modelBuilder.Entity<GameDetail>()
                .HasOne(x => x.Environment)
                .WithMany()
                .IsRequired();



            //Because there are multiple Fkeys to the same table for both HeroTeams and Villain teams, the relations are defined here.
            modelBuilder.Entity<HeroTeam>()
                .HasOne(x => x.First)
                .WithMany(c => c.FirstPosition)
                .IsRequired();
            modelBuilder.Entity<HeroTeam>()
                .HasOne(x => x.Second)
                .WithMany(c => c.SecondPosition)
                .IsRequired();
            modelBuilder.Entity<HeroTeam>()
                .HasOne(x => x.Third)
                .WithMany(c => c.ThirdPosition)
                .IsRequired();
            modelBuilder.Entity<HeroTeam>()
                .HasOne(x => x.Fourth)
                .WithMany(c => c.FourthPosition);
            modelBuilder.Entity<HeroTeam>()
                .HasOne(x => x.Fifth)
                .WithMany(c => c.FifthPosition);

            modelBuilder.Entity<VillainTeam>()
                .HasOne(x => x.First)
                .WithMany(c => c.FirstPosition)
                .IsRequired();
            modelBuilder.Entity<VillainTeam>()
                .HasOne(x => x.Second)
                .WithMany(c => c.SecondPosition);
            modelBuilder.Entity<VillainTeam>()
                .HasOne(x => x.Third)
                .WithMany(c => c.ThirdPosition);
            modelBuilder.Entity<VillainTeam>()
                .HasOne(x => x.Fourth)
                .WithMany(c => c.FourthPosition);
            modelBuilder.Entity<VillainTeam>()
                .HasOne(x => x.Fifth)
                .WithMany(c => c.FifthPosition);

            //Users and UserPermissions are a 1 to 1 relationship, this defines the parent/child

            modelBuilder.Entity<User>()
                .HasOne(x => x.UserPermission)
                .WithOne(y => y.User)
                .HasForeignKey<UserPermission>(y => y.UserId);

        }

    }


}
