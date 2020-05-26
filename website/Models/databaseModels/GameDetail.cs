using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class GameDetail
    {
        public int ID { get; set; }


        //FKey for Games
        public int GameId { get; set; }
        public Game Game { get; set; }

        //FKey for VillainTeams (1 to many)
        public int VillainTeamId { get; set; }
        public VillainTeam VillainTeam { get; set; }


        //Fkey for HeroTeams (1 to many)
        public int HeroTeamId { get; set; }
        public HeroTeam HeroTeam { get; set; }

        //Fkey for GameEnvironments (1 to many)
        [ForeignKey("Environment")]
        [Display(Name="Environment: ")]
        public GameEnvironment Environment { get; set; }

        //Gotta figure out how to do this...
        public int HeroPositionsIncap { get; set; }
        public int VillainPostionsIncap { get; set; }

        public int PerceivedDifficulty { get; set; }

        //should be an enum.
        public string SelectionMethod { get; set; }

        public int NumberOfPlayers { get; set; }
        public int GameTimeLength { get; set; }
        public int Rounds { get; set; }
        public bool Win { get; set; }

        //Another table?
        public string GameEndCondition { get; set; }

        public string UserComment { get; set; }

        //Calculated field? Or Record the calculation at time of entry...
        public double CalculatedDifficulty { get; set; }

    }
}
