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

        //Fkey the many side of a 1 to many.
        public int UserID { get; set; }
        public User User { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime AttemptTime { get; set; }
        public string IPAddress { get; set; }
    }
}
