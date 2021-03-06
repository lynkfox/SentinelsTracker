﻿using System;
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
        //null because there may not be a user associated with this.
        public int? UserId { get; set; }
        public User User { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateEntered { get; set; }


        //Fkey from GameDetails (1 to 1 relationship)
        public GameDetail GameDetail { get; set; }

    }
}
