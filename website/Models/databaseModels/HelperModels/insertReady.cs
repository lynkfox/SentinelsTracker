using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels.HelperModels
{
    // a single model for inserting multiple models into the database from the Google Sheets.
    public class insertReady
    {

        public User User { get; set; }
        public Game Game { get; set; }
        public GameDetail GameDetail { get; set; }
        public List<HeroTeam>  HeroesUsed { get; set; }
        public List<VillainTeam> VillainsUsed { get; set; }
        public List<GameEnvironment> EnvironmentsUsed { get; set; }

    }
}
