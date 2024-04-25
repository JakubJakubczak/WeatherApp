using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeatherApplication.Data;
using WeatherApplication.Models;

namespace WeatherApplication.Controllers
{
    public class UbraniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UbraniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ubranies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ubranie.ToListAsync());
        }

        // GET: Ubranies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubranie = await _context.Ubranie
                .FirstOrDefaultAsync(m => m.id == id);
            if (ubranie == null)
            {
                return NotFound();
            }

            return View(ubranie);
        }

        // GET: Ubranies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ubranies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,rodzaj_ubrania,kolor,opis")] Ubranie ubranie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ubranie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ubranie);
        }

        // GET: Ubranies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubranie = await _context.Ubranie.FindAsync(id);
            if (ubranie == null)
            {
                return NotFound();
            }
            return View(ubranie);
        }

        // POST: Ubranies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,rodzaj_ubrania,kolor,opis")] Ubranie ubranie)
        {
            if (id != ubranie.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ubranie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UbranieExists(ubranie.id))
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
            return View(ubranie);
        }

        // GET: Ubranies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubranie = await _context.Ubranie
                .FirstOrDefaultAsync(m => m.id == id);
            if (ubranie == null)
            {
                return NotFound();
            }

            return View(ubranie);
        }

        // POST: Ubranies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ubranie = await _context.Ubranie.FindAsync(id);
            if (ubranie != null)
            {
                _context.Ubranie.Remove(ubranie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UbranieExists(int id)
        {
            return _context.Ubranie.Any(e => e.id == id);
        }
    }
}
