using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FolderTree.Models;
using FolderTree.Data;

namespace FolderTree.Controllers
{
    public class FoldersController : Controller
    {
        private readonly AppDbContext _context;

        public FoldersController(AppDbContext context)
        {
            _context = context;
        }

        // Initial data creation
        public async Task<IActionResult> InitialDataCreation()
        {
            var dbContext = _context;
            await dbContext.AddRangeAsync(InitialData.InitialDataListGeneration());
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Folders
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Folders.Include(f => f.ParrentFolder);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Folders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Folders == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .Include(f => f.ParrentFolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // GET: Folders/Create
        public IActionResult Create()
        {
            ViewData["ParrentId"] = new SelectList(_context.Folders, "Id", "Id");
            return View();
        }

        // POST: Folders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParrentId")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(folder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParrentId"] = new SelectList(_context.Folders, "Id", "Id", folder.ParrentId);
            return View(folder);
        }

        // GET: Folders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Folders == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            ViewData["ParrentId"] = new SelectList(_context.Folders, "Id", "Id", folder.ParrentId);
            return View(folder);
        }

        // POST: Folders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParrentId")] Folder folder)
        {
            if (id != folder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(folder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolderExists(folder.Id))
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
            ViewData["ParrentId"] = new SelectList(_context.Folders, "Id", "Id", folder.ParrentId);
            return View(folder);
        }

        // GET: Folders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Folders == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .Include(f => f.ParrentFolder)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Folders == null)
            {
                return Problem("Entity set 'AppDbContext.Folders'  is null.");
            }
            var folder = await _context.Folders.FindAsync(id);
            if (folder != null)
            {
                _context.Folders.Remove(folder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FolderExists(int id)
        {
          return (_context.Folders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
