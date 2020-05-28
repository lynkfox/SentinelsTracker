using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    [Table("PasswordHistories", Schema = "logging")]
    public class PasswordHistory
    {
        [Key]
        public int ID { get; set; }

        //FKey to user - 1 to Many
        public int UserId { get; set; }
        public User User { get; set; }

        public string PreviousSalt { get; set; }
        public string PreviousHash { get; set; }
        public DateTime ChangedOn { get; set; }

    }
}
