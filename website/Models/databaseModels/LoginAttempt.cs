using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    [Table("LoginAttempts", Schema = "logging")]
    public class LoginAttempt
    {
        [Key]
        public int ID { get; set; }
        public int? UserID { get; set; }
        public User User { get; set; }
        public DateTime AttemptTime { get; set; }
        public string IPAddress { get; set; }
    }
}
