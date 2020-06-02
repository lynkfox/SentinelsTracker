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
        [Required]
        public string Username { get; set; }
        [StringLength(500)]
        public string Profile { get; set; }
        [StringLength(75)]
        public string UserIcon { get; set; }

        [Index(IsUnique = true)]
        [StringLength(250)]
        public string UserEmail { get; set; }


        //This will be set to False if they have not yet claimed their games on this new platform. New users will always have this set to True.
        public bool HasClaimed { get; set; }


        /*Security concerns.
         * 
         * Look. I am no security expert and I certainly am not good enough to really foil anything. I also have no experience beyond these simple things I'm doing here
         * 
         * So. If you have the experience and want to help, please do! Fork and Request a merge when you got it much better.
         * 
         * Hopefully as a simple fan site this won't ever be a huge target! :) Their are simple procedures being followed, that will help, and the only personal information
         * is email address so ... hopefully it's OK!
         * 
         */

        public bool Locked { get; set; }
        public UserPermission UserPermission { get; set; }



        //Fkeys - 1 side to 1 to many
        public ICollection<Game> Games { get; set; }
        public ICollection<PasswordHistory> PasswordHistories { get; set; }
        public ICollection<LoginAttempt> LoginAttempts { get; set; }
        public ICollection<AccessChange> AccessChanges { get; set; }
    }
}
