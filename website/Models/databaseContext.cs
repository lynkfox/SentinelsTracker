using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
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
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }



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


            //Users and UserPermissions are a 1 to 1 relationship, this defines the parent/child

            modelBuilder.Entity<User>()
                .HasOne(x => x.UserPermission)
                .WithOne(y => y.User)
                .HasForeignKey<UserPermission>(y => y.UserId);


            
        }

    }


}
