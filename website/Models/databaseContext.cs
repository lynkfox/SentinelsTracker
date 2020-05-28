﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using website.Models.databaseModels;

namespace website.Models
{
    public class databaseContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameDetail> GameDetails { get; set; }
        public DbSet<GameEnvironment> Environments { get; set; }
        public DbSet<HeroTeam> HeroeTeams { get; set; }
        public DbSet<VillainTeam> VillainTeams { get; set; }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Villain> Villains { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<AccessLevel> AccessLevels { get; set; }

    }

    
}