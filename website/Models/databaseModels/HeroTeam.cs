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

        // Each Hero in the team is a FKey to the Hero character table

        [ForeignKey("FirstHero")]
        public int? FirstHeroId { get; set; }
        public Hero FirstHero { get; set; }


        [ForeignKey("SecondHero")]
        public int? SecondHeroId { get; set; }
        public Hero SecondHero { get; set; }

        [ForeignKey("ThirdHero")]
        public int? ThirdHeroId { get; set; }
        public Hero ThirdHero { get; set; }

        [ForeignKey("FourthHero")]
        public int? FourthHeroId { get; set; }
        public Hero FourthHero { get; set; }

        [ForeignKey("FifthHero")]
        public int? FifthHeroId { get; set; }
        public Hero FifthHero { get; set; }









    }
}
