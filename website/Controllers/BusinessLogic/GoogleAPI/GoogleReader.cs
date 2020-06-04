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
