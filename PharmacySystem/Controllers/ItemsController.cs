using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacySystem.Data;
using PharmacySystem.Models1;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace PharmacySystem.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ItemsController : Controller
    {
        private readonly PharmcySystemContext _context;

        public ItemsController(PharmcySystemContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            return _context.Items != null ?
                        View(await _context.Items.ToListAsync()) :
                        Problem("Entity set 'PharmcySystemContext.Items'  is null.");
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Quantity,Price,ImageData,ImageName")] Item item, IFormFile? files)
        {
            if (ModelState.IsValid)
            {
                if (files != null && files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);

                    item.ImageName = fileName;

                    using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        item.ImageData = target.ToArray();
                    }
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Quantity,Price,ImageData,ImageName")] Item item, IFormFile? files)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (files != null && files.Length > 0)
                    {
                        //Getting FileName
                        var fileName = Path.GetFileName(files.FileName);

                        item.ImageName = fileName;

                        using (var target = new MemoryStream())
                        {
                            files.CopyTo(target);
                            item.ImageData = target.ToArray();
                        }
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Update(item);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ID))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'PharmcySystemContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return (_context.Items?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
