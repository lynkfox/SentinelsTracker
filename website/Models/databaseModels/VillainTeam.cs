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

        //If this team villain is actually OblivAeon, and then the various Villains should point to Scions (OblivAeon is assumed)
        public bool OblivAeon { get; set; }

        // Each Villain in the team is a FKey to the Villain character table. Most Second on will be Null

        [ForeignKey("FirstVillain")]
        [Required]
        public Villain First { get; set; }
        [ForeignKey("SecondVillain")]
        public Villain Second { get; set; }
        [ForeignKey("ThirdVillain")]
        public Villain Third { get; set; }
        [ForeignKey("FourthVillain")]
        public Villain Fourth { get; set; }
        [ForeignKey("FifthVillain")]
        public Villain Fifth { get; set; }
    }
}
