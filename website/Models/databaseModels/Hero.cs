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

        [Required]
        [Display(Name = "Hero Name: ")]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Team { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(75)]
        public string WikiLink { get; set; }

        [Required]
        [Range(1,5)]
        public int PrintedComplexity { get; set; }
        public bool IsAlt { get; set; }

        //Name of the base hero if this is an alt. Null if none
        [Display(Name = "Variant Of: ")]
        [StringLength(250)]
        public string BaseHero { get; set; }

        //Foreign Key for BoxSet 1 to Many (The many side)
        public int BoxSetId { get; set; }
        public BoxSet BoxSet { get; set; }

        [StringLength(75)]
        public string Image { get; set; }

        //fkey the one side of a  one to many.
        public ICollection<HeroTeam> HeroTeams { get; set; }

    }
}
