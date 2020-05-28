using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    public class HeroTeam
    {
        public int ID { get; set; }

        // Each Hero in the team is a FKey to the Hero character table

        [ForeignKey("FirstHero")]
        public Hero First { get; set; }
        [ForeignKey("SecondHero")]
        public Hero Second { get; set; }
        [ForeignKey("ThirdHero")]
        public Hero Third { get; set; }
        [ForeignKey("FourthHero")]
        public Hero Fourth { get; set; }
        [ForeignKey("FifthHero")]
        public Hero Fifth { get; set; }
    }
}
