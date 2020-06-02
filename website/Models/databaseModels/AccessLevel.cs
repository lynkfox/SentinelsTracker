using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    [Table("AccessLevels", Schema = "users")]
    public class AccessLevel
    {
        [Key]
        public int Level { get; set; }
        public bool Read { get; set; }
        public bool Modify { get; set; }
        public string LevelCommonName { get; set; }


        //Fkeys - This is the 1 side to the 1 to many
        public ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<AccessChange> AccessChanges { get; set; }
    }
}
