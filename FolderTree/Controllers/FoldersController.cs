using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FolderTree.Models;
using FolderTree.Data;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace FolderTree.Controllers
{
    public class FoldersController : Controller
    {
        private readonly AppDbContext _context;

        public FoldersController(AppDbContext context)
        {
            _context = context;
        }

        // Initial data creation if db is empty
        public async Task<IActionResult> InitialDataCreation()
        {
            if (_context.Folders.ToList().Count == 0)
            {   
				await _context.AddRangeAsync(InitialData.InitialDataListGeneration());
				await _context.SaveChangesAsync();
			}
            return RedirectToAction(nameof(ShowSubFolders));
        }

        // GET: SubFolders
        public async Task<IActionResult> ShowSubFolders(int? id = 1) 
        {
			if (id == null || _context.Folders == null)
			{
				return NotFound();
			}

            List<Folder> subFoldersList = await _context.Folders
                .Where(f => f.ParrentId == id)
                .ToListAsync();

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

        // Import folders tree
        public async Task<FileResult> ExportFoldersTree()
        {
            var export = await _context.Folders.Select(x => x).ToListAsync();

			JsonSerializerOptions options = new()
			{
				ReferenceHandler = ReferenceHandler.IgnoreCycles,
				WriteIndented = true
			};

			string exportJson = JsonSerializer.Serialize(export, options);

            byte[] exportBytes = Encoding.Default.GetBytes(exportJson);
            
            return File(exportBytes, "text/plain", "foldertree.txt");
        }
    }
}
