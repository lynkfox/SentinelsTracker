using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("EnvironmentsUsed", Schema = "statistics")]
    public class EnvironmentUsed
    {
        public int ID { get; set; }

        //Fkey - One to Many. This is the many side
        public int GameDetailId { get; set; }
        public GameDetail GameDetail { get; set; }


        //Fkey - 1 to many. This is the many side
        public int GameEnvironmentId { get; set; }
        public GameEnvironment GameEnvironment { get; set; }


        //For OblivAeon Games - true means this was destroyed during the game.
        public bool Destroyed { get; set; }
        //For OblivAeon Games - Zone 0 for non OblivAeon. 1 or 2 for OblivAeon
        public int OblivAeonZone { get; set; }
        //For OblivAeon Games - order in which this environment appeared in the zone above. 0 for first (or non OblivAeon) 1 for 2nd, ect
        public int OblivAeonOrderAppeared { get; set; }

    }
}
