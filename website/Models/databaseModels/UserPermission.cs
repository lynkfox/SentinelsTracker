using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class UserPermission
    {
        public int id { get; set; }
        public int UserID { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public AccessLevel AccessLevel{ get; set; }
    }
}
