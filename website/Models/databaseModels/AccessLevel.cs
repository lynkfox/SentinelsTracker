using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class AccessLevel
    {
        public int Level { get; set; }
        public bool Read { get; set; }
        public bool Modify { get; set; }
    }
}
