using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using website.Controllers.BusinessLogic.GoogleReader;
using website.Models;
using website.Models.databaseModels;

namespace website.Controllers
{
    public class HomeController : Controller
    {


        private GoogleRead sheetReader = new GoogleRead();
        private readonly DatabaseContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;


        static readonly string SpreadsheetId = "185wr2Ws6D_8IAIsxIzxniN7tDwEm0IXsOZEm5lR9rMI";
        //The link of the BoxSets Sheet



        public HomeController(DatabaseContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _db = context;
            _env = env;
            _config = config;
        }



        /*Summary
        * Initialize the DB with the Primer Sheet -- To be removed from live
        */


        public async Task<IActionResult> GoogleLoad()
        {

            var webRoot = _env.WebRootPath; // find the file on the server
            string directoryPath = Path.Combine(webRoot, "Data", "app_client_secret.json"); // This will need to be in official secrets not just an open json :p

            sheetReader.Init(directoryPath);
           
            _db.AddOrUpdateRange<BoxSet>(sheetReader.ReadBoxes(SpreadsheetId));
            await _db.SaveChangesAsync();

            var existingBoxes = _db.BoxSet.ToList();
            _db.AddOrUpdateRange<GameEnvironment>(sheetReader.ReadEnvironment(SpreadsheetId, existingBoxes));
            _db.AddOrUpdateRange<Hero>(sheetReader.ReadHero(SpreadsheetId, existingBoxes));
            _db.AddOrUpdateRange<Villain>(sheetReader.ReadVillain(SpreadsheetId, existingBoxes));


            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }





        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
