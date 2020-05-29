using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("VillainCharacters", Schema = "gamedata")]
    public class Villain
    {
        public int ID { get; set; }

        [Display(Name = "Villan Name: ")]
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string WikiLink { get; set; }
        public int PrintedDifficulty { get; set; }

        //Fkey to Box Set 1 to many
        public int BoxSetId { get; set; }
        public BoxSet BoxSet { get; set; }

        public string Image { get; set; }

        [InverseProperty("FirstVillain")]
        public ICollection<VillainTeam> FirstPosition { get; set; }

        [InverseProperty("SecondVillain")]
        public ICollection<VillainTeam> SecondPosition { get; set; }

        [InverseProperty("ThirdVillain")]
        public ICollection<VillainTeam> ThirdPosition { get; set; }

        [InverseProperty("FourthVillain")]
        public ICollection<VillainTeam> FourthPosition { get; set; }

        [InverseProperty("FifthVillain")]
        public ICollection<VillainTeam> FifthPosition { get; set; }
    }
}
