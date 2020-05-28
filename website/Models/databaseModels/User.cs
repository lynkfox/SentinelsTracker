using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace website.Models.databaseModels
{
    [Table("Users", Schema = "users")]
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


        /*Security concerns.
         * 
         * Look. I am no security expert and I certainly am not good enough to really foil anything. I also have no experience beyond these simple things I'm doing here
         * 
         * So. If you have the experience and want to help, please do! Fork and Request a merge when you got it much better.
         * 
         * Luckily, this is a niche fan site that I can't see as getting very big or dangerous! :)
         */
        public UserPermission UserPermission { get; set; }

        public ICollection<Game> Games { get; set; }
        public ICollection<PasswordHistory> PasswordHistories { get; set; }
        public ICollection<LoginAttempt> LoginAttempts { get; set; }
    }
}
