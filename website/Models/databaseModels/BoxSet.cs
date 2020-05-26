using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class BoxSet
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PublicationDate { get; set; }
        public string WikiLink { get; set; }
        public string Image { get; set; }
    }
}
