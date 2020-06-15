using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using website.Controllers.BusinessLogic;
using website.Controllers.BusinessLogic.GoogleReader;
using website.Models;
using website.Models.databaseModels;

namespace website.Controllers
{
    public class HeroesController : Controller
    {
        private GoogleRead sheetReader = new GoogleRead();
        private readonly DatabaseContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;


        static readonly string SpreadsheetId = "185wr2Ws6D_8IAIsxIzxniN7tDwEm0IXsOZEm5lR9rMI";
        public HeroesController(DatabaseContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _db = context;
            _env = env;
            _config = config;
        }

        public async Task<IActionResult> GoogleLoad()
        {

            var webRoot = _env.WebRootPath; // find the file on the server
            string directoryPath = Path.Combine(webRoot, "Data", "app_client_secret.json"); // This will need to be in official secrets not just an open json :p

            sheetReader.Init(directoryPath);

            var existingBoxes = _db.BoxSet.ToList();

            


            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Heroes
        public async Task<IActionResult> Index()
        {
            var databaseContext = _db.Heroes.Include(h => h.BoxSet);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Heroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hero = await _db.Heroes
                .Include(h => h.BoxSet)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hero == null)
            {
                return NotFound();
            }

            return View(hero);
        }

        // GET: Heroes/Create
        public IActionResult Create()
        {
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name");
            return View();
        }

        // POST: Heroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Team,Description,WikiLink,PrintedComplexity,IsAlt,BaseHero,BoxSetId,Image")] Hero hero)
        {
            if (ModelState.IsValid)
            {
                _db.Add(hero);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", hero.BoxSetId);
            return View(hero);
        }

        // GET: Heroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hero = await _db.Heroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", hero.BoxSetId);
            return View(hero);
        }

        // POST: Heroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Team,Description,WikiLink,PrintedComplexity,IsAlt,BaseHero,BoxSetId,Image")] Hero hero)
        {
            if (id != hero.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(hero);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeroExists(hero.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", hero.BoxSetId);
            return View(hero);
        }

        // GET: Heroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hero = await _db.Heroes
                .Include(h => h.BoxSet)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hero == null)
            {
                return NotFound();
            }

            return View(hero);
        }

        // POST: Heroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hero = await _db.Heroes.FindAsync(id);
            _db.Heroes.Remove(hero);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeroExists(int id)
        {
            return _db.Heroes.Any(e => e.ID == id);
        }
    }
}
