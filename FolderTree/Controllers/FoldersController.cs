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

        // GET: SubFolders
        public async Task<IActionResult> ShowSubFolders(int? id) 
        {
			if (id == null || _context.Folders == null)
			{
				return NotFound();
			}

            List<Folder> subFoldersList = await _context.Folders.Where(f => f.ParrentId == id).ToListAsync();

			var folder = await _context.Folders
				.FirstOrDefaultAsync(m => m.Id == id);
			if (folder == null)
			{
				return NotFound();
			}

			folder.SubFolders = subFoldersList;

            await _context.SaveChangesAsync();
            			
			return View(folder);
        }

        // GET: Folders
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Folders.Include(f => f.ParrentFolder);
            return View(await appDbContext.ToListAsync());
        }

        private bool FolderExists(int id)
        {
          return (_context.Folders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
