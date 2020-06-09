using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using website.Models;
using website.Models.databaseModels;
using website.Models.databaseModels.HelperModels;

namespace website.Controllers.BusinessLogic.GoogleReader

{
    public class GoogleRead
    {
        readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        readonly string ApplicationName = "SentinelTracker";
        readonly string sheet = "BoxSets";
        private SheetsService service;



        private Dictionary<string, int> VillainIDs = new Dictionary<string, int>()
        {
            {"Aeon Master", 1},
            {"Akash'Bhuta",2},
            {"Ambuscade",3},
            {"Apostate",5},
            {"Baron Blade",6},
            {"Baron Blade, Mad Bomber",7},
            {"Baron Blade: Vengeance Five",8},
            {"Biomancer",9},
            {"Borr the Unstable",10},
            {"Bugbear",11},
            {"La Capitan",12},
            {"Chairman, The",14},
            {"Chokepoint",15},
            {"Citizen Dawn",16},
            {"Citizens Hammer and Anvil",17},
            {"Dark Mind",18},
            {"Deadline",19},
            {"Dreamer, The",20},
            {"Empyreon",21},
            {"Ennead, The",22},
            {"Ermine",23},
            {"Faultless",24},
            {"Friction",25},
            {"Fright Train",26},
            {"Gloomweaver",27},
            {"Gloomweaver, Skinwalker",28},
            {"Grand Warlord Voss",29},
            {"Ranek Kel'Voss",30},
            {"Grezer Clutch",31},
            {"Infinitor",32},
            {"Infinitor: Tormented Ally",33},
            {"Iron Legacy",34},
            {"Kaargra Warfang",35},
            {"Kismet",36},
            {"Kismet, Trickster",37},
            {"Matriarch, The",38},
            {"Miss Information",39},
            {"Nixious The Chosen",41},
            {"OblivAeon",42},
            {"Omnitron",43},
            {"Omnitron, Cosmic",44},
            {"Operative, The",45},
            {"Plague Rat",46},
            {"Progeny",48},
            {"Proletariat",50},
            {"Sanction",51},
            {"Sergeant Steel",52},
            {"Spite",53},
            {"Spite: Agent of Gloom",54},
            {"Void Soul",55},
            {"Wager Master",56},
            {"Anathema",57},
            {"Anathema, Evolved",58},
            {"Dendron",59},
            {"Dendron, Windcolor",60},
            {"Gray",61},
            {"Ram, The",62},
            {"Ram, The: 1929",63},
            {"Tiamat",64},
            {"Tiamat, Hydra",65},
            {"Oriphel",66},
            {"Swarm Eater",67},
            {"Swarm Eater, Hivemind",68},
            {"Vector",69},
            {"Phase",70},
            {"Celadroch",71},
            {"Menagerie",72},
            {"Dynamo",73},
            {"Infernal Choir, The",74},
            {"Mistress of Fate, The",75},
            {"Mythos",76},
            {"Outlander",77},
            {"Screamachine",78}
        };


        //Set Up the Connection to the google API
        public void Init(string directoryPath)
        {
            GoogleCredential credential;


            //Read the credentials file in.
            using (var stream = new FileStream(directoryPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);

            }

            //Create the API Service that can access everything in the sheet.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName

            });


        }

        

        public ValueRange GetValues(string SpreadsheetId, string sheetName)
        {
            var range = $"{sheetName}!A:AJ";

            //sets the Request we are going to use with Execute, with the Spreadsheet ID and the Range
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(SpreadsheetId, range);

            //Captures the Sheet
            var response = request.Execute();
            return response;
        }


        //Game Entry Record Read

        public List<insertReady> ConvertFromGoogleToModel(string spreadsheetID)
        {
            var inserts = new List<insertReady>();


            IList<IList<object>> values = GetValues(spreadsheetID, "TestEntry").Values;

            foreach(var row in values)
            {
                if(row[0].ToString() == "Timestamp")
                {
                    //Title row. skip
                    continue;
                }

                var user = new User()
                {
                    //if the last boxes in the row are all empty, Google doesn't add empty entries to the array
                    //So if it exists, but is empty then Anonymous, otherwise Anonymous.
                    Username = row.Count > 34 ?  string.IsNullOrEmpty(row[34].ToString()) ? "Anonymous" : row[34].ToString() : "Anonymous", 
                    Profile = null,
                    UserIcon = null,
                    UserEmail = "None"

                };

                var details = new GameDetail()
                {
                   // HeroTeams = RetrieveHeroTeam(row.Skip(17).Take(10))
                };



                var villainsUsed = new VillainTeam();

                var environments = new EnvironmentUsed();



                var game = new Game()
                {
                    UserId = null,
                    DateEntered = Convert.ToDateTime(row[0].ToString()),
                    GameDetail = details
                };

                

                

            }



            return inserts;
        }
        public ICollection<HeroTeam> RetrieveHeroTeam(IEnumerable<object> row)
        {
            //Hero Team is a multi side of a relationship table, so we'll make a list for each one and get it ready. The Hero will need to be found in the db before insertion.

            var heroTeam = new List<HeroTeam>();
            bool heroField = true;
            int position = 1;

            var hero = new Hero();
            

            foreach(var entry in row)
            {
                
                if(heroField)
                {
                    //Save the hero name
                    hero.Name = entry.ToString().Equals("(none)") ? null : entry.ToString();
                    
                    //Set it so the next pass through the iEnumberable will set the position/incap data
                    heroField = false;

                }
                else if (!string.IsNullOrEmpty(hero.Name)) // if the hero.Name has been set to null then there is no hero to add to the list.
                {
                    var teamMember = new HeroTeam()
                    {
                        Hero = hero,
                        Position = position,
                        Incapped = !string.IsNullOrEmpty(entry.ToString())
                    };
                    // Reset - increase position by 1, get a clean hero, and set heroField true so it will get the new hero
                    position++;
                    hero = new Hero();
                    heroField = true;

                    heroTeam.Add(teamMember);
                }
                
                
            }


            return heroTeam;
        }


        public ICollection<VillainTeam> RetrieveVillains(IEnumerable<object> row)
        {
            var villainTeam = new List<VillainTeam>();

            bool team = false;

            if(row.First().ToString() == "Vengeance Five")
            {
                //Setup
                team = true;
                int position = 1;

                //10 columns of possible villains, add them in sections of 2 to divide them up with their possible incaps.
                List<IEnumerable<object>> teamVillains = ExtractIndividualTeam(row);

                foreach (var member in teamVillains)
                {
                    if (string.IsNullOrEmpty(member.First().ToString()))
                    { // if there is no entry in the first then there was no villain so we can break early.
                        break;
                    }

                    string villainName = RemoveExtraVengeanceTag(member.First().ToString());

                    int villainCorrectedID = VillainIDCorrection(villainName);

                    var teamMember = new VillainTeam()
                    {
                        VillainId = villainCorrectedID,
                        Position = position,
                        Flipped = !string.IsNullOrEmpty(member.Last().ToString()),
                        VillainTeamGame = team
                    };

                    villainTeam.Add(teamMember);

                    position++;
                }

            }
            else // Solo Villain (not Team) 
            {

                var teamMember = new VillainTeam()
                {
                    VillainId = VillainIDs[row.First().ToString()],
                    Position = 1,
                    Flipped = !string.IsNullOrEmpty(row.Last().ToString()),
                    VillainTeamGame = team
                };

                villainTeam.Add(teamMember);

            }

            return villainTeam;
        }

        private List<IEnumerable<object>> ExtractIndividualTeam(IEnumerable<object> row)
        {
            List<IEnumerable<object>> teamMembers = new List<IEnumerable<object>>();

            for (int i = 1; i < 10; i += 2)
            {
                teamMembers.Add(row.Skip(i).Take(2));
            }

            return teamMembers;
        }


        /* Edge Cases
         * 
         * 
         * There are some edge cases and unique situations that need to be adjusted for from how the
         *Team Villains are displayed in the Google Docs and how they are in the Database (cleaner in the db)
         *
         * Team Villains have " (Vengeance)" that needs to be removed
         * 
         * The Database allows duplicate names (bit cleaner for other aspects) and so the proper ID's for the Team Version need to be recovered
         */
        private string RemoveExtraVengeanceTag(string villainName)
        {
            return villainName.Replace(" (Vengeance)", "");
        }

        private int VillainIDCorrection(string villainName)
        {
            /* Edge cases (duplicate names in database)
             * 
             * Ambuscade (team)
             * La Capitan (Team)
             * Baron Blade: Vengeance Five (team)
             * Miss Information (team)
             * Plague Rat (team)
             * Progeny (Scion)
             * 
             * since the names have been added with specified ID's we can (and are) assuming the team version of the same villain is +1
             * 
             * (excepting Baron Blade because he has 2 additionals, but they have different names)
             */

            if(villainName == "Ambuscade" ||
                villainName == "La Capitan" || 
                villainName == "Miss Information" ||
                villainName == "Plague Rat" ||
                villainName == "Progeny")
            {
                return VillainIDs[villainName] + 1;
            }

            else if(villainName == "Baron Blade")
            {
                return VillainIDs["Baron Blade: Vengeance Five"];
            }
            else
            {
                return VillainIDs[villainName];
            }

        }


        

        /* These functions read in the game data from the primer doc.
         * 
         * They were partially created to get a sense on how to use the Google Sheet API,
         * but also for use when first deploying. After the database is up and running and primed, these should be removed in
         * future live environments
         * 
         */


        //Game Data Reads. 
        public List<BoxSet> ReadBoxes(string SpreadsheetId)
        {

            var boxSetsFromGoogleSheets = new List<BoxSet>();
            //puts the values of each column(?)row? into a list of lists of objects
            IList<IList<object>> values = GetValues(SpreadsheetId, "BoxSets").Values;

            bool firstRow = true;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    if (firstRow)
                    {
                        //Skip Column Headers
                        firstRow = false;
                        continue;
                    }



                    BoxSet newBox = new BoxSet()
                    {
                        ID = int.Parse(row[0].ToString()),
                        Name = row[1].ToString(),
                        Description = row[2].ToString(),
                        PublicationDate = row[3].ToString(),
                        WikiLink = row[4].ToString(),
                        Image = "default"
                    };

                    boxSetsFromGoogleSheets.Add(newBox);


                }
            }

            return boxSetsFromGoogleSheets;
        }

        internal List<Hero> ReadHero(string SpreadsheetId, List<BoxSet> boxSetsFromDB)
        {
            var heroFromGoogleSheet = new List<Hero>();
            IList<IList<object>> values = GetValues(SpreadsheetId, "Heroes").Values;

            bool firstRow = true;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    if (firstRow)
                    {
                        //Skip Column Headers
                        firstRow = false;
                        continue;
                    }

                    Hero hero = new Hero()
                    {
                        ID = int.Parse(row[0].ToString()),
                        Name = row[1].ToString(),
                        Team = string.IsNullOrEmpty(row[2].ToString()) ? null : row[2].ToString(),
                        Description = row[3].ToString(),
                        WikiLink = row[4].ToString(),
                        PrintedComplexity = int.Parse(row[5].ToString()),
                        IsAlt = bool.Parse(row[6].ToString()),
                        BaseHero = string.IsNullOrEmpty(row[7].ToString()) ? null : row[7].ToString(),
                        BoxSet = boxSetsFromDB.Where(x => x.Name == row[8].ToString()).FirstOrDefault(),
                        Image = "default"

                    };


                    heroFromGoogleSheet.Add(hero);

                }
            }

            return heroFromGoogleSheet;
        }

        internal List<Villain> ReadVillain(string SpreadsheetId, List<BoxSet> boxSetsFromDB)
        {
            var villainFromGoogleSheet = new List<Villain>();
            IList<IList<object>> values = GetValues(SpreadsheetId, "Villains").Values;

            bool firstRow = true;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    if (firstRow)
                    {
                        //Skip Column Headers
                        firstRow = false;
                        continue;
                    }

                    Villain villain = new Villain()
                    {
                        ID = int.Parse(row[0].ToString()),
                        Name = row[1].ToString(),
                        Type = (Villain.Types)Enum.Parse(typeof(Villain.Types),row[2].ToString()),
                        BaseName = string.IsNullOrEmpty(row[3].ToString()) ? null : row[3].ToString(),
                        Description = row[4].ToString(),
                        WikiLink = row[5].ToString(),
                        PrintedDifficulty = int.Parse(row[6].ToString()),
                        BoxSet = boxSetsFromDB.Where(x => x.Name == row[7].ToString()).First(),
                        Image = "default"

                    };


                    villainFromGoogleSheet.Add(villain);

                }
            }

            return villainFromGoogleSheet;
        }

        public List<GameEnvironment> ReadEnvironment(string SpreadsheetId, List<BoxSet> boxSetsFromDB)
        {
            var environFromGoogleSheet = new List<GameEnvironment>();
            IList<IList<object>> values = GetValues(SpreadsheetId, "Environments").Values;

            bool firstRow = true;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    if (firstRow)
                    {
                        //Skip Column Headers
                        firstRow = false;
                        continue;
                    }



                    GameEnvironment gameEnvironment = new GameEnvironment()
                    {
                        ID = int.Parse(row[0].ToString()),
                        Name = row[1].ToString(),
                        Description = row[2].ToString(),
                        WikiLink = row[3].ToString(),
                        BoxSet = boxSetsFromDB.Where(x => x.Name == row[4].ToString()).FirstOrDefault(),
                        Image = "default"
                    };

                    environFromGoogleSheet.Add(gameEnvironment);


                }
            }

            return environFromGoogleSheet;
        }

        
    }

    
}
