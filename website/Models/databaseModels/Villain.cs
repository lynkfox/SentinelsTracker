using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("VillainCharacters", Schema = "gamedata")]
    public class Villain : IGameData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Villan Name: ")]
        [StringLength(250)]
        public string Name { get; set; }


        [Required]
        public Types Type { get; set; }

        //The name of the original this is a vairant of. Null if none
        [Display(Name = "Variant of: ")]
        [StringLength(250)]
        public string BaseName { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(75)]
        public string WikiLink { get; set; }
        [StringLength(75)]
        public string Image { get; set; }

        [Required]
        public int PrintedDifficulty { get; set; }

        //Fkey to Box Set 1 to many
        public int BoxSetId { get; set; }
        public BoxSet BoxSet { get; set; }

        //Fkey the one side of a one to many.
        public ICollection<VillainTeam> VillainTeams { get; set; }


        public enum Types
        {
            Solo,
            Alt,
            Team,
            Scion,
            OblivAeon
        }


        //Interface options for Generic Methods
        [NotMapped]
        object IGameData.ID { get { return ID; } }
    }
}

