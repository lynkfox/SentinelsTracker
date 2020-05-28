using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class LoginAttempt
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public User User { get; set; }
        public DateTime AttemptTime { get; set; }
    }
}
