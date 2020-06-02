using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using website.Models;
using website.Models.databaseModels;

namespace website.Controllers.BusinessLogic.GoogleAPI

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

        private ValueRange GetValues(string SpreadsheetId, string sheetName)
        {
            var range = $"{sheetName}!A:L";

            //sets the Request we are going to use with Execute, with the Spreadsheet ID and the Range
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(SpreadsheetId, range);

            //Captures the Sheet
            var response = request.Execute();
            return response;
        }


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
