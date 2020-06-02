using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    [Table("BoxSets", Schema = "gamedata")]
    public class BoxSet
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(25)]
        public string PublicationDate { get; set; }
        [StringLength(75)]
        public string WikiLink { get; set; }
        [StringLength(75)]
        public string Image { get; set; }


        //Fkeys - this is the 1 side to the 1 to many
        public ICollection<Hero> Heroes { get; set; }
        public ICollection<Villain> Villains { get; set; }
        public ICollection<GameEnvironment> GameEnvironments { get; set; }
    }
}
