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
    public class BoxSetsController : Controller
    {

        private GoogleRead boxSetReader = new GoogleRead();
        private readonly DatabaseContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;


        static readonly string SpreadsheetId = "185wr2Ws6D_8IAIsxIzxniN7tDwEm0IXsOZEm5lR9rMI";
        //The link of the BoxSets Sheet



        public BoxSetsController(DatabaseContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _db = context;
            _env = env;
            _config = config;
        }


       










        // GET: BoxSets
        public async Task<IActionResult> Index()
        {

            return View(await _db.BoxSet.ToListAsync());
        }

        // GET: BoxSets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boxSet = await _db.BoxSet
                .FirstOrDefaultAsync(m => m.ID == id);
            if (boxSet == null)
            {
                return NotFound();
            }

            return View(boxSet);
        }

        // GET: BoxSets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BoxSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,PublicationDate,WikiLink,Image")] BoxSet boxSet)
        {
            if (ModelState.IsValid)
            {
                _db.Add(boxSet);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boxSet);
        }

        // GET: BoxSets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boxSet = await _db.BoxSet.FindAsync(id);
            if (boxSet == null)
            {
                return NotFound();
            }
            return View(boxSet);
        }

        // POST: BoxSets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,PublicationDate,WikiLink,Image")] BoxSet boxSet)
        {
            if (id != boxSet.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(boxSet);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoxSetExists(boxSet.ID))
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
            return View(boxSet);
        }

        // GET: BoxSets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boxSet = await _db.BoxSet
                .FirstOrDefaultAsync(m => m.ID == id);
            if (boxSet == null)
            {
                return NotFound();
            }

            return View(boxSet);
        }

        // POST: BoxSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boxSet = await _db.BoxSet.FindAsync(id);
            _db.BoxSet.Remove(boxSet);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoxSetExists(int id)
        {
            return _db.BoxSet.Any(e => e.ID == id);
        }
    }
}