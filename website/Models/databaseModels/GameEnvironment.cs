using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    public class GameEnvironment
    {
        [Key]
        [Display(Name = "Environment: ")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string WikiLink { get; set; }

        //FKey for Box Set, 1 to Many
        public int BoxSetId { get; set; }
        public BoxSet BoxSet { get; set; }

        public ICollection<GameDetail> GameDetails { get; set; }
    }
}
