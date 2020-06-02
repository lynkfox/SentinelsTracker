using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using website.Controllers.BusinessLogic.GoogleAPI;
using website.Models;
using website.Models.databaseModels;

namespace website.Controllers
{
    public class GameEnvironmentsController : Controller
    {
        private GoogleRead sheetReader = new GoogleRead();
        private readonly DatabaseContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;


        static readonly string SpreadsheetId = "185wr2Ws6D_8IAIsxIzxniN7tDwEm0IXsOZEm5lR9rMI";
        



        public GameEnvironmentsController(DatabaseContext context, IWebHostEnvironment env, IConfiguration config)
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

            _db.AddOrUpdateRange<GameEnvironment>(sheetReader.ReadEnvironment(SpreadsheetId, existingBoxes));

            _db.AddRange();

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        // GET: GameEnvironments
        public async Task<IActionResult> Index()
        {
            return View(await _db.GameEnvironments.ToListAsync());
        }

        // GET: GameEnvironments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameEnvironment = await _db.GameEnvironments
                .Include(g => g.BoxSet)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gameEnvironment == null)
            {
                return NotFound();
            }

            return View(gameEnvironment);
        }

        // GET: GameEnvironments/Create
        public IActionResult Create()
        {
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name");
            return View();
        }

        // POST: GameEnvironments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,WikiLink,Image,BoxSetId")] GameEnvironment gameEnvironment)
        {
            if (ModelState.IsValid)
            {
                _db.Add(gameEnvironment);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", gameEnvironment.BoxSetId);
            return View(gameEnvironment);
        }

        // GET: GameEnvironments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameEnvironment = await _db.GameEnvironments.FindAsync(id);
            if (gameEnvironment == null)
            {
                return NotFound();
            }
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", gameEnvironment.BoxSetId);
            return View(gameEnvironment);
        }

        // POST: GameEnvironments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,WikiLink,Image,BoxSetId")] GameEnvironment gameEnvironment)
        {
            if (id != gameEnvironment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(gameEnvironment);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameEnvironmentExists(gameEnvironment.ID))
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
            ViewData["BoxSetId"] = new SelectList(_db.BoxSet, "ID", "Name", gameEnvironment.BoxSetId);
            return View(gameEnvironment);
        }

        // GET: GameEnvironments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameEnvironment = await _db.GameEnvironments
                .Include(g => g.BoxSet)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (gameEnvironment == null)
            {
                return NotFound();
            }

            return View(gameEnvironment);
        }

        // POST: GameEnvironments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameEnvironment = await _db.GameEnvironments.FindAsync(id);
            _db.GameEnvironments.Remove(gameEnvironment);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameEnvironmentExists(int id)
        {
            return _db.GameEnvironments.Any(e => e.ID == id);
        }
    }
}
