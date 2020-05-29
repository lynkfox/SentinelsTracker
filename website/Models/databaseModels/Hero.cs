using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("HeroCharacters", Schema = "gamedata")]
    public class Hero
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Hero Name: ")]
        public string Name { get; set; }
        public string Team { get; set; }
        public string Description { get; set; }
        public string WikiLink { get; set; }
        public int PrintedComplexity { get; set; }
        public bool IsAlt { get; set; }

        //Foreign Key for BoxSet 1 to Many
        public int BoxSetId { get; set; }
        public BoxSet BoxSet { get; set; }

        public string Image { get; set; }



        [InverseProperty("FirstHero")]
        public ICollection<HeroTeam> FirstPosition { get; set; }

        [InverseProperty("SecondHero")]
        public ICollection<HeroTeam> SecondPosition { get; set; }

        [InverseProperty("ThirdHero")]
        public ICollection<HeroTeam> ThirdPosition { get; set; }

        [InverseProperty("FourthHero")]
        public ICollection<HeroTeam> FourthPosition { get; set; }

        [InverseProperty("FifthHero")]
        public ICollection<HeroTeam> FifthPosition { get; set; }
    }
}
