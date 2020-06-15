using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
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

        /* Hardcoded ids! The HORROR!
         * 
         * In this case we have 2 reasons for this
         * 
         * 1)  We are forcing ID's on entry for the Heroes/Environments/Boxes/Villains when priming the database. This is to keep things in a certain order in
         * in regards to the Cauldron and OrderBy(ID) gives us the order we want no matter which we use (name may not always, especially with some alternates)
         * 
         * 2) Hardcoded here, even though it opens a possiblity for bugs in a problematic ID, is far faster. This dictionary loaded into memory won't need to access
         * Entity Framework in order to get the ID and we can move through the thousands of entries much faster (hopefully)
         */

        private Dictionary<string, int> HeroIDs = new Dictionary<string, int>()
        {
            { "Absolute Zero" , 1 },
            { "Absolute Zero: Termi-Nation" , 2 },
            { "Absolute Zero: Freedom Five" , 3 },
            { "Absolute Zero: Freedom Six (Elemental Wrath)" , 4 },
            { "Akash'Thriya" , 5 },
            { "Akash'Thriya: Spirit of the Void" , 6 },
            { "Argent Adept" , 7 },
            { "Argent Adept: Prime Warden" , 8 },
            { "Argent Adep, The: Dark Conductor (Kvothe)" , 9 },
            { "Argent Adept: XTREME Prime Warden" , 10 },
            { "Benchmark" , 11 },
            { "Benchmark: Supply and Demand" , 12 },
            { "Bunker" , 13 },
            { "Bunker: GI" , 14 },
            { "Bunker: Termi-Nation" , 15 },
            { "Bunker: Freedom Five" , 16 },
            { "Bunker: Freedom Six (Engine of War)" , 17 },
            { "Captain Cosmic" , 18 },
            { "Captain Cosmic: Prime Warden" , 19 },
            { "Captain Cosmic: Requital" , 20 },
            { "Captain Cosmic: XTREME Prime Warden" , 21 },
            { "Chrono-Ranger" , 22 },
            { "Chrono-Ranger: Best of Times" , 23 },
            { "La Comodora" , 24 },
            { "La Comodora: Curse of the Black Spot" , 25 },
            { "Dr. Medico: Void Guard" , 26 },
            { "Dr. Medico: Void Guard, Malpractice" , 27 },
            { "Expatriette" , 28 },
            { "Expatriette: Dark Watch" , 29 },
            { "Fanatic" , 30 },
            { "Fanatic: Redeemer" , 31 },
            { "Fanatic: Prime Warden" , 32 },
            { "Fanatic: XTREME Prime Warden" , 33 },
            { "Guise" , 34 },
            { "Guise, Santa" , 35 },
            { "Guise, Completionist" , 36 },
            { "Haka" , 37 },
            { "Haka: Prime Warden" , 38 },
            { "Haka: XTREME Prime Warden" , 39 },
            { "Haka: Eternal" , 40 },
            { "Harpy" , 41 },
            { "Harpy: Dark Watch" , 42 },
            { "Idealist: Void Guard" , 43 },
            { "Idealist: Void Guard, Super Sentai" , 44 },
            { "K.N.Y.F.E." , 45 },
            { "K.N.Y.F.E.: Rogue Agent" , 46 },
            { "Legacy" , 47 },
            { "Legacy: Young" , 48 },
            { "Legacy: Greatest" , 49 },
            { "Legacy: Freedom Five" , 50 },
            { "Lifeline" , 52 },
            { "Lifeline: Bloodmage" , 53 },
            { "Luminary" , 54 },
            { "Luminary: Heroic (Ivana)" , 55 },
            { "Mainstay: Void Guard" , 56 },
            { "Mainstay: Void Guard, Road Warrior" , 57 },
            { "Mr. Fixer" , 58 },
            { "Mr. Fixer: Dark Watch" , 59 },
            { "Naturalist" , 60 },
            { "Naturalst: Hunted" , 61 },
            { "Nightmist" , 62 },
            { "Nightmist: Dark Watch" , 63 },
            { "Omnitron-X" , 64 },
            { "Omnitron-X: Omnitron-U" , 65 },
            { "Parse" , 66 },
            { "Parse: Fugue State" , 67 },
            { "Ra" , 68 },
            { "Ra: Horus of the Two Horizons" , 69 },
            { "Ra: Setting Sun" , 70 },
            { "Scholar" , 71 },
            { "Scholar: Of the Infinite, The" , 72 },
            { "The Sentinels", 73 },
            { "The Sentinels: Adamant" , 74 },
            { "Setback" , 75 },
            { "Setback: Darkwatch" , 76 },
            { "Sky-Scraper" , 77 },
            { "Sky-Scraper: Extremist" , 78 },
            { "Stuntman" , 79 },
            { "Stuntman: Action Hero" , 80 },
            { "Tachyon" , 81 },
            { "Tachyon: Super Scientific" , 82 },
            { "Tachyon: Freedom Five" , 83 },
            { "Tachyon: Freedom Six (Team Leader)" , 84 },
            { "Tempest" , 85 },
            { "Tempest: Prime Warden" , 86 },
            { "Tempest: Freedom Six (Sacrifice)" , 87 },
            { "Tempest: XTREME Prime Warden" , 88 },
            { "Unity" , 89 },
            { "Unity: Termi-Nation" , 90 },
            { "Unity: Freedom Six (Golem)" , 91 },
            { "Visionary" , 92 },
            { "Visionary: Dark" , 93 },
            { "Visionary: Unleashed" , 94 },
            { "Wraith" , 95 },
            { "Wraith: Rook City" , 96 },
            { "Wraith: Freedom Five" , 97 },
            { "Wraith: Freedom Six (Price of Freedom)" , 98 },
            { "Writhe: Void Guard" , 99 },
            { "Writhe: Cosmic Inventor" , 100 },
        };

        private Dictionary<string, int> VillainIDs = new Dictionary<string, int>()
        {
            {"Akash'Bhuta",2},
            {"Ambuscade",3},
            {"Ambuscade (Vengeance)", 4},
            {"Apostate",5},
            {"Baron Blade",6},
            {"Mad Bomber Blade",7},
            {"Baron Blade (Vengeance)", 8},
            {"Biomancer (Vengeance)",9},
            {"Bugbear (Vengeance)",11},
            {"La Capitan",12},
            {"La Capitan (Vengeance)",13},
            {"The Chairman",14},
            {"Chokepoint",15},
            {"Citizen Dawn",16},
            {"Citizens Hammer and Anvil (Vengeance)",17},
            {"Deadline",19},
            {"The Dreamer",20},
            {"The Ennead",22},
            {"Ermine (Vengeance)",23},
            {"Friction (Vengeance)",25},
            {"Fright Train (Vengeance)",26},
            {"Gloomweaver",27},
            {"Skinwalker Gloomweaver",28},
            {"Grand Warlord Voss",29},
            {"Grezer Clutch (Vengeance)",31},
            {"Infinitor",32},
            {"Tormented Infinitor",33},
            {"Iron Legacy",34},
            {"Kaargra Warfang",35},
            {"Kismet",36},
            {"Unstable Kismet",37},
            {"The Matriarch",38},
            {"Miss Information",39},
            {"Miss Information (Vengeance)",40},
            {"Nixious The Chosen",41},
            {"Omnitron",43},
            {"Cosmic Omnitron",44},
            {"The Operative (Vengeance)",45},
            {"Plague Rat",46},
            {"Plague Rat (Vengeance)",46},
            {"Progeny",48},
            {"Proletariat (Vengeance)",50},
            {"Sergeant Steel (Vengeance)",52},
            {"Spite",53},
            {"Agent of Gloom Spite",54},
            {"Wager Master",56}
        };

        private Dictionary<string, int> EnvironmentIds = new Dictionary<string, int>()
        {
            { "The Block" , 2 },
            { "Celestial Tribunal" , 2 },
            { "Champion Studios" , 3 },
            { "The Court Of Blood" , 4 },
            { "Dok'Thorath" , 5 },
            { "Enclave of the Endlings" , 6 },
            { "The Final Wasteland" , 7 },
            { "Fort Adamant" , 8 },
            { "Freedom Tower" , 9 },
            { "Insula Primalis" , 10 },
            { "Madame Mittermeier's" , 11 },
            { "Magmaria" , 13 },
            { "Maerynian Refuge" , 12 },
            { "Mobile Defense Platform" , 14 },
            { "Mordengrad" , 15 },
            { "Nexus of the Void" , 16 },
            { "Omintron IV" , 17 },
            { "Pike Industrial Complex" , 18 },
            { "Realm of Discord" , 19 },
            { "Ruins of Atlantis" , 20 },
            { "Silver Gulch 1883" , 21 },
            { "Temple of Zhu Long" , 22 },
            { "Time Cataclysm" , 23 },
            { "Tomb of Anubis" , 24 },
            { "Wagner Mars Base" , 25 }
        };

        private Dictionary<string, GameDetail.GameEndConditions> EndGameConditionConversion = new Dictionary<string, GameDetail.GameEndConditions>()
        {
            { "" , GameDetail.GameEndConditions.IncapVillain },
            { "HP Incapacitation (Heroes)" , GameDetail.GameEndConditions.IncapHeroes },
            { "Terra Lunar Impulsion Beam (Baron Blade)" , GameDetail.GameEndConditions.TerraLunarImpulsionBeam },
            { "The Environment was Destroyed (Deadline)" , GameDetail.GameEndConditions.WorldBurns },
            { "Did Not Protect Her (The Dreamer)" , GameDetail.GameEndConditions.DidNotProtectHer },
            { "Relic Victory (Gloomweaver)" , GameDetail.GameEndConditions.RelicVictory },
            { "Ate Himself (Skinwalker Gloomweaver)" , GameDetail.GameEndConditions.ConsumedFromWithin },
            { "Minion Overrun (Voss)" , GameDetail.GameEndConditions.MinionOverrun },
            { "The Crowd Turned Against the Heroes (Kaargra Warfang)" , GameDetail.GameEndConditions.CrowdTurns },
            { "Omnitron's Devices After Omnitron died" , GameDetail.GameEndConditions.OmnitronLivesOn },
            { "Wager Master Alternate Win Condition" , GameDetail.GameEndConditions.WagerWin },
            { "Wager Master Alternate Loose Condition" , GameDetail.GameEndConditions.WagerLoss },
            { "OblivAeonBetrayed" , GameDetail.GameEndConditions.OblivAeonBetrayed },
            { "Sentenced to Destruction (Celestial Tribunal)" , GameDetail.GameEndConditions.Sentanced },
            { "Engines Failed (Mobile Defense Platform)" , GameDetail.GameEndConditions.EnginesFailed },
            { "Time Portal Closed (Silver Gulch 1883 )" , GameDetail.GameEndConditions.PortalClosed },
            { "Mars Base Explosion (Wagner)" , GameDetail.GameEndConditions.SelfDestruct },
            { "Environment Card Killed Villain" , GameDetail.GameEndConditions.Environment },
            { "Sucker Punch, Final Dive, and other Destroy Cards" , GameDetail.GameEndConditions.Destroyed },
            { "Incapacitated Hero Ability" , GameDetail.GameEndConditions.IncapAbility },
            { "Other" , GameDetail.GameEndConditions.Other }
        };

        private Dictionary<string, GameDetail.SelectionMethods> SelectionMethodConversion = new Dictionary<string, GameDetail.SelectionMethods>()
        {
            { "Player Choice", GameDetail.SelectionMethods.PlayerChoice },
            { "Randomizer Program", GameDetail.SelectionMethods.Random },
            { "Dice", GameDetail.SelectionMethods.Random },
            { "SotM Video Game Random Button", GameDetail.SelectionMethods.Random },
            { "Thematic Team, Villain, Environment", GameDetail.SelectionMethods.Thematic },
            { "Scenario", GameDetail.SelectionMethods.Thematic },
            { "Rematch", GameDetail.SelectionMethods.Rematch },
            { "", GameDetail.SelectionMethods.Other }
        };

        private Dictionary<string, GameDetail.Platforms> PlatformDetailConversion = new Dictionary<string, GameDetail.Platforms>()
        {
            { "Physical Cards", GameDetail.Platforms.Physical },
            { "Steam", GameDetail.Platforms.Steam },
            { "Tablet (iPad or Android)", GameDetail.Platforms.Mobile } //for iOS/Android both phones and Tablets.
        };

        private Dictionary<string, GameDetail.GameTimeLengths> TimeLengthConversion = new Dictionary<string, GameDetail.GameTimeLengths>()
        {
            { "<30" , GameDetail.GameTimeLengths.LessThanThirty },
            { "30-44" , GameDetail.GameTimeLengths.ThirtyToFortyFour } ,
            { "45-59" ,  GameDetail.GameTimeLengths.FortyFiveToFiftyNine } ,
            { "60-90" , GameDetail.GameTimeLengths.OneHourToOneAndHalfHours} ,
            { "90+" , GameDetail.GameTimeLengths.MoreThanOneAndHalfHours },
            { "" , GameDetail.GameTimeLengths.Unmarked } 
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


        /* Game Entry Record Read
         * 
         * This is the main method to read a row in the google sheet and prepare it for database entry. With almost 35k entries we have to figure out the most 
         * effecient way to do this
         * 
         * not currently loving a Foreach over every single row but ... I don't think there is another way :/
         * 
         * Might only read in a thousand at a time?
         */

        public List<Game> ConvertFromGoogleToModel(string spreadsheetID)
        {
            var inserts = new List<Game>();


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
                    //So if it exists, but is empty then Anonymous, otherwise it doesnt exist so Anonymous, otherwise it exists and has a value so Value.
                    Username = row.Count > 34 ? string.IsNullOrEmpty(row[34].ToString()) ? "Anonymous" : row[34].ToString() : "Anonymous",
                    Profile = null,
                    UserIcon = null,
                    UserEmail = "None",
                    HasClaimed = false

                };



                var details = new GameDetail()
                {
                    HeroTeams = CreateHeroTeams(ExtractHeroTeam(row.Skip(17).Take(10))),
                    VillainTeams = CreateVillainTeams(ExtractVillainTeam(row.Skip(1).Take(15))),
                    EnvironmentsUsed = CreateEnvironmentsUsed(row[27].ToString()),
                    GameMode = ExtractGameMode(row.Skip(13).Take(2)),
                    GameEndCondition = EndGameCond(row[16]),
                    Win = row[12].ToString() == "Yes",
                    SelectionMethod = SelMethod(row),
                    PerceivedDifficulty = PerceivedDiff(row),
                    GameTimeLength = TimeToPlay(row),
                    NumberOfPlayers = NumberOfPlayers(row),
                    Rounds = Rounds(row),
                    UserComment = Comments(row),
                    CalculatedDifficulty = 0,
                };

                var game = new Game()
                {
                    User = user,
                    DateEntered = Convert.ToDateTime(row[0].ToString()),
                    GameDetail = details
                };

                inserts.Add(game);

            }

            return inserts;
        }




        /* The following methods take smaller portions of the entire row and work with it for setting upt he GameDetails
         */



        public ICollection<HeroTeam> CreateHeroTeams(List<(string name, bool flip)> teamMembers)
        {
            //Hero Team is a multi side of a relationship table, so we'll make a list for each one and get it ready. The Hero will need to be found in the db before insertion.

            var heroTeam = new List<HeroTeam>();
            int position = 1;


            foreach(var member in teamMembers)
            {
                if(member.name == "(none)")
                { // (none) indicates no one in that position, and as they are supposed to be in order... (might need todouble check that)
                    break;
                }
                var teamMember = new HeroTeam()
                {
                    HeroId = HeroIDs[member.name],
                    Position = position,
                    Incapped = member.flip
                };
                // Reset - increase position by 1, get a clean hero, and set heroField true so it will get the new hero
                position++;
                heroTeam.Add(teamMember);
            }


            return heroTeam;
        }

        

        public ICollection<VillainTeam> CreateVillainTeams(List<(string name, bool flip)> teamVillains)
        {
            var villainTeam = new List<VillainTeam>();

            
            int position = 1;

         
            //if more than one in Team Villains list then team is true.
            bool team = teamVillains.Count > 1;

            foreach (var member in teamVillains)
            {
                if (string.IsNullOrEmpty(member.name))
                { // if there is no entry in the first then there was no villain so we can break early.
                    break;
                } else if (member.name == "Vengeance Five")
                {
                    
                    continue;
                }

                
                var teamMember = new VillainTeam()
                {
                    VillainId = VillainIDs[member.name],
                    Position = position,
                    Flipped = member.flip,
                    VillainTeamGame = team
                };

                villainTeam.Add(teamMember);

                position++;
            }

            
            

            return villainTeam;
        }

        public List<(string name, bool flip)> ExtractVillainTeam(IEnumerable<object> row)
        {
            var teamMembers = new List<(string name, bool flip)>();
            var columns = row.ToList();

            //add the Solo Villain or Vengeance Team qualifier

            var soloOrFirst = (name: columns[0].ToString(), flip: !string.IsNullOrEmpty(columns[14].ToString()));
            teamMembers.Add(soloOrFirst);

            //add the team memberes if there are any

            if(!string.IsNullOrEmpty(columns[1].ToString()))
            {
                for (int i = 1; i < 10; i += 2)
                {
                    var nextTeamMember = (name: columns[i].ToString(), flip: !string.IsNullOrEmpty(columns[i + 1].ToString()));

                    teamMembers.Add(nextTeamMember);
                }
            }

            return teamMembers;
        }

        public List<(string name, bool flip)> ExtractHeroTeam(IEnumerable<object> row)
        {
            var teamMembers = new List<(string name, bool flip)>();
            var columns = row.ToList();

            for (int i = 0; i < 10; i += 2)
            {
                var memberStatus = (name: columns[i].ToString(), flip: !string.IsNullOrEmpty(columns[i + 1].ToString()));

                teamMembers.Add(memberStatus);
            }

            return teamMembers;
        }

        public ICollection<EnvironmentUsed> CreateEnvironmentsUsed(string row)
        {
            var environmentsUsed = new List<EnvironmentUsed>();


            //EnvironmentsUsed are a Many from a 1:Many relationship in the database due to OblivAeon being able to destroy them.
            //However there are no OblivAeon games in the database, so we just have to fake it for the database.
            environmentsUsed.Add(new EnvironmentUsed
            {
                GameEnvironmentId = EnvironmentIds[row],
                Destroyed = false,
                OblivAeonOrderAppeared = 0,
                OblivAeonZone = 0
            });

            return environmentsUsed;
        }


        public GameDetail.GameModes ExtractGameMode(IEnumerable<object> row)
        {
            bool advanced = row.First().ToString() == "Yes";
            bool challenge = !string.IsNullOrEmpty(row.Last().ToString());

            string type = advanced && challenge ? "Ultimate"
                        : advanced ? "Advanced"
                        : challenge ? "Challenge"
                        : "Normal";

            return (GameDetail.GameModes)Enum.Parse(typeof(GameDetail.GameModes), type);
        }




        /* ==============================================================================================================================
         * 
         * The next few methods are for converting from the sheets (which were set for Readability) into various Enums and values.
         * that are better suited for internal structures with Entity Framework and Databases.
         * 
         * So we'll use a dictionary to get the proper enum.
         * 
         * 
         * It is also possible for any or all of these fields to "not exist" because of the way the google API pulls, so there needs to be a check for if the row even
         * contains the value we're looking for.
         * 
         * They are mostly here to make the main method up above look cleaner.
         * 
         * ==============================================================================================================================
         */

        public GameDetail.GameEndConditions EndGameCond(object entry)
        {
            
            return EndGameConditionConversion[entry.ToString()];
        }

        public GameDetail.SelectionMethods SelMethod(IList<object> entry)
        {
            return entry.Count <= 28 ?GameDetail.SelectionMethods.Other : SelectionMethodConversion[entry[28].ToString()];
        }


        public int PerceivedDiff(IList<object> entry)
        {
            return entry.Count <= 29 || string.IsNullOrEmpty(entry[29].ToString()) ? 0 : int.Parse(entry[29].ToString());
        }

        public GameDetail.GameTimeLengths TimeToPlay(IList<object> entry)
        {
            return entry.Count <= 30 ? GameDetail.GameTimeLengths.Unmarked : TimeLengthConversion[entry[30].ToString()];
        }

        public int NumberOfPlayers(IList<object> entry)
        {
            return entry.Count <= 31 || string.IsNullOrEmpty(entry[31].ToString()) ? 0 : int.Parse(entry[31].ToString());
        }

        public int Rounds(IList<object> entry)
        {
            return entry.Count <= 32 || string.IsNullOrEmpty(entry[32].ToString()) ? 0 : int.Parse(entry[32].ToString());
        }

        public string Comments(IList<object> entry)
        {
            return entry.Count <= 35 || string.IsNullOrEmpty(entry[35].ToString()) ? null : entry[35].ToString(); 
        }


       






        

        /* ============================================================================================================
         * 
         * These functions read in the game data from the primer doc.
         * 
         * They were partially created to get a sense on how to use the Google Sheet API,
         * but also for use when first deploying. After the database is up and running and primed, these should be removed in
         * future live environments
         * 
         * ============================================================================================================
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
