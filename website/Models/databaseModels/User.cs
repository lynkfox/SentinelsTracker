using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Profile { get; set; }
        public string UserIcon { get; set; }
        public List<Game> Games { get; set; }
        public string UserEmail { get; set; }
    }
}
