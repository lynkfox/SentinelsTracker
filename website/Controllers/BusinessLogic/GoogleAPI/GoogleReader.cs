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
            { "Argent Adept, The" , 7 },
            { "Argent Adept, The: Prime Wardens" , 8 },
            { "Argent Adep, The: Dark Conductor" , 9 },
            { "Argent Adept, The: XTREME Prime Wardens" , 10 },
            { "Benchmark" , 11 },
            { "Benchmark: Supply and Demand" , 12 },
            { "Bunker" , 13 },
            { "Bunker: GI Bunker" , 14 },
            { "Bunker: Termi-Nation" , 15 },
            { "Bunker: Freedom Five" , 16 },
            { "Bunker: Freedom Six (Engine of War)" , 17 },
            { "Captain Cosmic" , 18 },
            { "Captain Cosmic: Prime Wardens" , 19 },
            { "Captain Cosmic: Requital" , 20 },
            { "Captain Cosmic: XTREME Prime Wardens" , 21 },
            { "Chrono-Ranger" , 22 },
            { "Chrono-Ranger: The Best of Times" , 23 },
            { "Comodora, La" , 24 },
            { "Comodora, La: Curse of the Black Spot" , 25 },
            { "Dr. Medico: Void Guard" , 26 },
            { "Dr. Medico: Malpractice" , 27 },
            { "Expatriette" , 28 },
            { "Expatriette: Dark Watch" , 29 },
            { "Fanatic" , 30 },
            { "Fanatic: Redeemer" , 31 },
            { "Fanatic: Prime Wardens" , 32 },
            { "Fanatic: XTREME Prime Wardens" , 33 },
            { "Guise" , 34 },
            { "Guise, Santa" , 35 },
            { "Guise, Completionist" , 36 },
            { "Haka" , 37 },
            { "Haka: Prime Wardens" , 38 },
            { "Haka: XTREME Prime Wardens" , 39 },
            { "Haka: The Eternal" , 40 },
            { "Harpy, The" , 41 },
            { "Harpy, The: Dark Watch" , 42 },
            { "Idealist: Void Guard" , 43 },
            { "Idealist: Super Sentai" , 44 },
            { "K.N.Y.F.E." , 45 },
            { "K.N.Y.F.E.: Rogue Agent" , 46 },
            { "Legacy" , 47 },
            { "Legacy: America's Newest (Beacon)" , 48 },
            { "Legacy: America's Greatest" , 49 },
            { "Legacy: Freedom Five" , 50 },
            { "Legacy: America's Cleverist" , 51 },
            { "Lifeline" , 52 },
            { "Lifeline: Bloodmage" , 53 },
            { "Luminary" , 54 },
            { "Luminary: Ivana Ramonat" , 55 },
            { "Mainstay: Void Guard" , 56 },
            { "Mainstay: Road Warrior" , 57 },
            { "Mr. Fixer" , 58 },
            { "Mr. Fixer: Dark Watch" , 59 },
            { "Naturalist, The" , 60 },
            { "Naturalst, The Hunted" , 61 },
            { "Nightmist" , 62 },
            { "Nightmist: Dark Watch" , 63 },
            { "Omnitron-X" , 64 },
            { "Omnitron-U" , 65 },
            { "Parse" , 66 },
            { "Parse: Fugue State" , 67 },
            { "Ra" , 68 },
            { "Ra: Horus of the Two Horizons" , 69 },
            { "Ra: The Setting Sun" , 70 },
            { "Scholar, The" , 71 },
            { "Scholar of the Infinite, The" , 72 },
            { "Sentinels, The" , 73 },
            { "Sentinels, The Adamant" , 74 },
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
            { "Tempest: Prime Wardens" , 86 },
            { "Tempest: Freedom Six (Sacrifice)" , 87 },
            { "Tempest: XTREME Prime Wardens" , 88 },
            { "Unity" , 89 },
            { "Unity: Termi-Nation" , 90 },
            { "Unity: Freedom Six (Golem)" , 91 },
            { "Visionary, The" , 92 },
            { "Visionary, Dark" , 93 },
            { "Visionary, The: Unleashed" , 94 },
            { "Wraith, The" , 95 },
            { "Wraith, The Rook City" , 96 },
            { "Wraith, The: Freedom Five" , 97 },
            { "Wraith, The: Freedom Six (Price of Freedom)" , 98 },
            { "Writhe: Void Guard" , 99 },
            { "Writhe: Cosmic Inventor" , 100 },
            { "Baccarat" , 101 },
            { "Baccarat: Ace of Swords" , 102 },
            { "Baccarat: Ace of Sorrows" , 103 },
            { "Baccarat: 1929" , 104 },
            { "Doc Havoc" , 105 },
            { "Doc Havoc: First Response" , 106 },
            { "Doc Havoc: 2199" , 107 },
            { "Knight, The" , 108 },
            { "Knight, The Fair" , 109 },
            { "Knight, The Berserker" , 110 },
            { "Knight, The: 1929" , 111 },
            { "Knight, The: Wasteland Ronin" , 112 },
            { "Lady of the Wood" , 113 },
            { "Lady of the Wood: Season of Change" , 114 },
            { "Lady of the Wood: Ministry of Strategic Science" , 115 },
            { "Lady of the Wood: 2199" , 116 },
            { "Malichae" , 117 },
            { "Malichae: Shardmaster" , 118 },
            { "Malichae: Ministry of Strategic Science" , 119 },
            { "Necro" , 120 },
            { "Necro: Warden of Chaos" , 121 },
            { "Necro: 1929" , 122 },
            { "Quicksilver" , 123 },
            { "Quicksilver, The Uncanny" , 124 },
            { "Quicksilver: Renegade" , 125 },
            { "Starlight" , 126 },
            { "Starlight: Genesis" , 127 },
            { "Starlight: Nightlore Council" , 128 },
            { "Stranger, The" , 129 },
            { "Stranger, The Rune-Carved" , 130 },
            { "Stranger, The: 1929" , 131 },
            { "Stranger, The: Wasteland Ronin" , 132 },
            { "Tango One" , 133 },
            { "Tango One: Ghost Ops" , 134 },
            { "Tango One: 1929" , 135 },
            { "Vanish" , 136 },
            { "Vanish: First Response" , 137 },
            { "Vanish: 1929" , 138 },
            { "Cricket" , 139 },
            { "Cricket: First Response" , 140 },
            { "Cricket: Renegade" , 141 },
            { "Cricket: Wasteland Ronin" , 142 },
            { "Cypher" , 143 },
            { "Cypher: First Response" , 144 },
            { "Cypher: Swarming Protocol" , 145 },
            { "Titan" , 146 },
            { "Titan: Ministry of Strategic Science" , 147 },
            { "Titan: 2199" , 148 },
            { "Echelon" , 149 },
            { "Echelon: First Response" , 150 },
            { "Echelon: 2199" , 151 },
            { "Impact" , 152 },
            { "Impact: Renegade" , 153 },
            { "Impact: Wasteland Ronin" , 154 },
            { "Magnificent Mara" , 155 },
            { "Magnificent Mara: 1929" , 156 },
            { "Magnificent Mara: Ministry of Strategic Science" , 157 },
            { "Drift" , 158 },
            { "Drift: Through the Breach" , 159 },
            { "Drift: 1929/2199" , 160 },
            { "Gargoyle" , 161 },
            { "Gargoyle: 2199" , 162 },
            { "Gargoyle: Wasteland Ronin" , 163 },
            { "Gyrosaur" , 164 },
            { "Gyrosaur: Speed Demon" , 165 },
            { "Gyrosaur: Renegade" , 166 },
            { "Pyre" , 167 },
            { "Pyre, The Unstable" , 168 },
            { "Pyre: Wasteland Ronin" , 169 },
            { "Terminus" , 170 },
            { "Terminus: 2199" , 171 },
            { "Terminus: Ministry of Strategic Science" , 172 }
        };

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

        private Dictionary<string, int> EnvironmentIds = new Dictionary<string, int>()
        {
            { "Block, The" , 2 },
            { "Celestial Tribunal, The" , 2 },
            { "Champion Studios" , 3 },
            { "Court Of Blood, The" , 4 },
            { "Dok'Thorath Capital" , 5 },
            { "Enclave of the Endlings" , 6 },
            { "The Final Wasteland" , 7 },
            { "Fort Adamant" , 8 },
            { "Freedom Tower" , 9 },
            { "Insula Primalis" , 10 },
            { "Madame Mittermeier's Fantastical Festival of Conundrums & Curiosities" , 11 },
            { "Magmeria" , 12 },
            { "Maerynian Refuge" , 13 },
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
            { "Wagner Mars " , 25 },
            { "Blackwood Forest" , 26 },
            { "F.S.C. Continuance Wanderer" , 27 },
            { "Halberd Experimental Research Center" , 28 },
            { "Northspar" , 29 },
            { "St. Simeon's Catacombs" , 30 },
            { "Wandering Isle" , 31 },
            { "Cybersphere" , 32 },
            { "Superstorm Akela" , 33 },
            { "Catchwater Harbor 1929" , 34 },
            { "Chasm of a Thousand Nights" , 35 },
            { "Nightlore Citadel" , 36 },
            { "Vault 5" , 37 },
            { "Windmill City" , 38 }
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
                   SelectionMethod = SelMethod(row),
                   PerceivedDifficulty = PerceivedDiff(row),
                   GameTimeLength = TimeToPlay(row)
                };



                



                var game = new Game()
                {
                    User = user,
                    DateEntered = Convert.ToDateTime(row[0].ToString()),
                    GameDetail = details
                };

                

                

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

                string villainName = RemoveExtraVengeanceTag(member.name);

                int villainCorrectedID = team ? VillainIDCorrection(villainName) : VillainIDs[member.name];
                
                var teamMember = new VillainTeam()
                {
                    VillainId = villainCorrectedID,
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

            var solo = (name: columns[0].ToString(), flip: !string.IsNullOrEmpty(columns[14].ToString()));
            teamMembers.Add(solo);

            //add the team memberes if there are any

            if(!string.IsNullOrEmpty(columns[1].ToString()))
            {
                for (int i = 1; i < 10; i += 2)
                {
                    var memberStatus = (name: columns[i].ToString(), flip: !string.IsNullOrEmpty(columns[i + 1].ToString()));

                    teamMembers.Add(memberStatus);
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




        /* The next few methods are for converting from the sheets (which were set for Readability) into various Enums and values.
         * that are better suited for internal structures with Entity Framework and Databases.
         * 
         * So we'll use a dictionary to get the proper enum.
         * 
         * 
         * It is also possible for any or all of these fields to "not exist" because of the way the google API pulls, so there needs to be a check for if the row even
         * contains the value we're looking for.
         * 
         * They are mostly here to make the main method up above look cleaner.
         */

        public GameDetail.GameEndConditions EndGameCond(object entry)
        {
            
            return EndGameConditionConversion[entry.ToString()];
        }

        public GameDetail.SelectionMethods SelMethod(IList<object> entry)
        {
            return entry.Count<28 ?GameDetail.SelectionMethods.Other : SelectionMethodConversion[entry[28].ToString()];
        }


        public int PerceivedDiff(IList<object> entry)
        {
            return entry.Count < 29 || string.IsNullOrEmpty(entry[29].ToString()) ? 0 : int.Parse(entry[29].ToString());
        }

        public GameDetail.GameTimeLengths TimeToPlay(IList<object> entry)
        {
            return entry.Count < 30 ? GameDetail.GameTimeLengths.Unmarked : TimeLengthConversion[entry[30].ToString()];
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
