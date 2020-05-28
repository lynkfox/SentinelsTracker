using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("HeroCharacters", Schema = "gamedata")]
    public class Hero
    {
        [Key]
        [Display(Name = "Hero Name: ")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string WikiLink { get; set; }
        public int PrintedComplexity { get; set; }
        
        //Foreign Key for BoxSet 1 to Many
        public int BoxSetId { get; set; }
        public BoxSet BoxSet { get; set; }


        public string Image { get; set; }

        public ICollection<HeroTeam> HeroTeams { get; set; }
    }
}
