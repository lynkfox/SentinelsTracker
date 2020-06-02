using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("HeroTeams", Schema = "statistics")]
    public class HeroTeam
    {
        public int ID { get; set; }

        //Fkey for GameDetails - 1 to many (This is the many side)
        public int GameDetailId { get; set; }
        public GameDetail GameDetail { get; set; }

        //FKey for Hero. 1 to many. (This is the many side)
        public int HeroId { get; set; }
        public Hero Hero { get; set; }

        //Position referes to Play Order - 1-5 (6 allowed for HouseRules of 6 player games)
        [Range(1,6)]
        [Required]
        public int Position { get; set; }

        //True indicates this hero was incapped during the game
        [Required]
        public bool Incapped { get; set; }

        //The order in which this hero was played during OblivAeon games - 0 would be the first hero played (or non OblivAeon Game).
        //1 would be the first hero to replace in the same position, 2 the next ect. 
        // So several heroes in a single OblivAeon game can have the same IncapOrder number - but need to have different positional numbers.
        public int OblivAeonIncapOrder { get; set; }











    }
}
