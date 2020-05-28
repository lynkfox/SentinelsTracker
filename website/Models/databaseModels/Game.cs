using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    [Table("Games", Schema = "statistics")]
    public class Game
    {

        [Key]
        [Display(Name = "Game ID Number: ")]
        public int ID { get; set; }

        //fkey for usersers (1 to many)
        public int UserId { get; set; }
        public User User { get; set; }


        public string Platform { get; set; }
        public DateTime DateEntered { get; set; }


        //Fkey from GameDetails (1 to 1 relationship)
        public GameDetail GameDetails { get; set; }

    }
}
