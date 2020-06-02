using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("GameEnvironments", Schema = "gamedata")]
    public class GameEnvironment
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Environment: ")]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(75)]
        public string WikiLink { get; set; }
        [StringLength(75)]
        public string Image { get; set; }

        //Fkey the many side of a 1 to many.
        public int BoxSetId { get; set; }
        public BoxSet BoxSet { get; set; }

        
        //Fkey - the one side of the one ot many.
        public ICollection<EnvironmentUsed> EnvironmentsUsed { get; set; }
        


    }
}
