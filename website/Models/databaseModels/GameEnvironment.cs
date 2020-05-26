using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public class GameEnvironment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WikiLink { get; set; }

        public ICollection<GameDetail> GameDetails { get; set; }
    }
}
