using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("VillainTeams", Schema = "statistics")]
    public class VillainTeam
    {
        public int ID { get; set; }

        //If this "team" is not a team at all but just a single villain, set false
        public bool VillainTeamGame { get; set; }
        //For team games - turn order 1-6 (6 for house rules). 1 or 0 is also used for Solo (Default to 0)
        public int Position { get; set; }
        public bool Flipped { get; set; }

        //If Any of the Villains attached to the same GameDetails show up as True in this field, then the controllers
        // will assume OblivAeon was also involved. This should only be set true for Scions.
        public bool OblivAeon { get; set; }

        public int VillainId { get; set; }
        public Villain Villain { get; set; }

        

    }
}
