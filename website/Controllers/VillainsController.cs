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
using website.Controllers.BusinessLogic.GoogleAPI;
using website.Models;
using website.Models.databaseModels;

namespace website.Controllers
{
    public class VillainsController : Controller
    {
        private GoogleRead sheetReader = new GoogleRead();
        private readonly DatabaseContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;


        static readonly string SpreadsheetId = "185wr2Ws6D_8IAIsxIzxniN7tDwEm0IXsOZEm5lR9rMI";
        public VillainsController(DatabaseContext context, IWebHostEnvironment env, IConfiguration config)
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

            _db.AddOrUpdateRange<Villain>(sheetReader.ReadVillain(SpreadsheetId, existingBoxes));


            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Villains
        public async Task<IActionResult> Index()
        {
            var databaseContext = _db.Villains.Include(v => v.BoxSet);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Villains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var villain = await _db.Villains
                .Include(v => v.BoxSet)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (villain == null)
            {
                return NotFound();
            }

            return View(villain);
        }

        // GET: Villains/Create
        public IActionResult Create()
        {
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name");
            return View();
        }

        // POST: Villains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Type,BaseName,Description,WikiLink,Image,PrintedDifficulty,BoxSetId")] Villain villain)
        {
            if (ModelState.IsValid)
            {
                _db.Add(villain);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", villain.BoxSetId);
            return View(villain);
        }

        // GET: Villains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var villain = await _db.Villains.FindAsync(id);
            if (villain == null)
            {
                return NotFound();
            }
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", villain.BoxSetId);
            return View(villain);
        }

        // POST: Villains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Type,BaseName,Description,WikiLink,Image,PrintedDifficulty,BoxSetId")] Villain villain)
        {
            if (id != villain.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(villain);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VillainExists(villain.ID))
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
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", villain.BoxSetId);
            return View(villain);
        }

        // GET: Villains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var villain = await _db.Villains
                .Include(v => v.BoxSet)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (villain == null)
            {
                return NotFound();
            }

            return View(villain);
        }

        // POST: Villains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var villain = await _db.Villains.FindAsync(id);
            _db.Villains.Remove(villain);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VillainExists(int id)
        {
            return _db.Villains.Any(e => e.ID == id);
        }
    }
}
