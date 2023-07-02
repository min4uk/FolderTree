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

		// export folders tree
		public async Task<FileResult> ExportFoldersTree()
		{
			List<Folder> export = await _context.Folders.Select(x => new Folder() { Id = x.Id, Name = x.Name, ParrentId = x.ParrentId }).ToListAsync();

			JsonSerializerOptions options = new()
			{
				WriteIndented = true
			};

			string exportJson = JsonSerializer.Serialize(export, options);

			byte[] exportBytes = Encoding.Default.GetBytes(exportJson);

			return File(exportBytes, "text/plain", "foldertree.txt");
		}

		// GET: importFoldersTree
		[HttpGet]
		public IActionResult ImportFoldersTree()
		{
			return View();
		}

		// import folders tree
		[HttpPost]
		public async Task<IActionResult> ImportFoldersTree(IFormFile file)
		{
			if (file == null || file.Length <= 0)
			{
				ViewBag.Message = "Uploading error";
				return View();
			}

			string fileContent = string.Empty;

			using (StreamReader reader = new StreamReader(file.OpenReadStream()))
			{
				fileContent = await reader.ReadToEndAsync();
			}

			List<Folder> exportList = JsonSerializer.Deserialize<List<Folder>>(fileContent);

			_context.Folders.RemoveRange(await _context.Folders.Select(x => x).ToListAsync());
			await _context.AddRangeAsync(exportList);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(ShowSubFolders));
		}
	}
}
