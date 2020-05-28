using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    public class User
    {
        public int ID { get; set; }

        [Index(IsUnique = true)]
        [StringLength(250)]
        public string Username { get; set; }

        public string Profile { get; set; }
        public string UserIcon { get; set; }

        [Index(IsUnique = true)]
        [StringLength(250)]
        public string UserEmail { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
