using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
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


        public List<BoxSet> ReadBoxes(string SpreadsheetId)
        {

            var boxSetsFromGoogleSheets = new List<BoxSet>();
            var range = $"{sheet}!A:E";

            //sets the Request we are going to use with Execute, with the Spreadsheet ID and the Range
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(SpreadsheetId, range);

            //Captures the Sheet
            var response = request.Execute();

            //puts the values of each column(?)row? into a list of lists of objects
            IList<IList<object>> values = response.Values;

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
                        Name = row[0].ToString(),
                        Description = row[1].ToString(),
                        PublicationDate = row[2].ToString(),
                        WikiLink = row[3].ToString(),
                        Image = "default"
                    };

                    boxSetsFromGoogleSheets.Add(newBox);


                }
            }

            return boxSetsFromGoogleSheets;
        }


    }
}
