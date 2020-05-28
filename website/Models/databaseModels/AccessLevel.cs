using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class AccessLevel
    {
        [Key]
        public int Level { get; set; }
        public bool Read { get; set; }
        public bool Modify { get; set; }
        public string LevelCommonName { get; set; }

        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
