using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    public class UserPermission
    {
        [Key]
        public int ID { get; set; }

        //Foreign Key to Users - 1 to 1 relationship
        public int UserId { get; set; }
        public User User { get; set; }

        //Password storage - May need to be updated to be more secure?
        public string Salt { get; set; }
        public string Hash { get; set; }

        //Foreign Key to Users 1 to Many relationship
        public int AccessLevelId { get; set; }
        public AccessLevel AccessLevel{ get; set; }
    }
}
