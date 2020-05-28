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
        public string Name { get; set; }
        public string Description { get; set; }
        public string PublicationDate { get; set; }
        public string WikiLink { get; set; }
        public string Image { get; set; }

        public ICollection<Hero> Heroes { get; set; }
        public ICollection<Villain> Villains { get; set; }
        public ICollection<GameEnvironment> GameEnvironments { get; set; }
    }
}
