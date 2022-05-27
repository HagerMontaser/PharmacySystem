using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacySystem.Data;
using PharmacySystem.Models;
using PharmacySystem.Models1;

namespace PharmacySystem.Controllers
{
    public class InvoicesController : Controller
    {
        static List<Item> items = new List<Item>();
        private readonly PharmcySystemContext _context;
        public InvoicesController(PharmcySystemContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Employee")]
        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var pharmcySystemContext = _context.Invoices.Include(i => i.Employee_UsernameNavigation);
            return View(await pharmcySystemContext.ToListAsync());
        }

        [Authorize(Roles = "Admin,Employee")]
        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Employee_UsernameNavigation)
                .FirstOrDefaultAsync(m => m.Invoice_No == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        [Authorize(Roles = "Admin,Employee")]
        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["Employee_Username"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            ViewData["Item_Name"] = new SelectList(_context.Items, "ID", "Name");
            return View();
        }

        [Authorize(Roles = "Admin,Employee")]
        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Invoice_No,Date,CustName,TotalPrice,Type,Employee_Username")]*/ InvoicesItems invoices_items)
        {
            if (ModelState.Root.Children[1].ValidationState!=null)
            {
                invoices_items.invoice.TotalPrice = (double)0;
                foreach(var item in items)
                {
                    invoices_items.invoice.TotalPrice += item.Price;
                }
                _context.Add(invoices_items.invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Employee_Username"] = new SelectList(_context.AspNetUsers, "Id", "UserName", invoices_items.invoice.Employee_Username);
            
            return View(invoices_items.invoice);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string Quantity ,InvoicesItems invoices_items)
        {
            ViewData["Employee_Username"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            ViewData["Item_Name"] = new SelectList(_context.Items, "ID", "Name" , invoices_items.item.ID);
            Item currentItem =await _context.Items.Where(a=>a.ID == invoices_items.item.ID).FirstOrDefaultAsync<Item>();
            double price = double.Parse(Quantity) * currentItem.Price;
            Item item = new Item();
            item.Name = currentItem.Name;
            item.Price = price;
            item.Quantity = Quantity;
            items.Add(item);
            ViewData["Items"] = items;
            return View("Create");
        }

        [Authorize(Roles = "Admin")]
        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["Type"] = invoice.Type;
            ViewData["Employee_Username"] = new SelectList(_context.AspNetUsers, "Id", "UserName", invoice.Employee_Username);
            return View(invoice);
        }

        [Authorize(Roles = "Admin")]
        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Invoice_No,Date,CustName,TotalPrice,Type,Employee_Username")] Invoice invoice)
        {
            if (id != invoice.Invoice_No)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Invoice_No))
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
            ViewData["Employee_Username"] = new SelectList(_context.AspNetUsers, "Id", "UserName", invoice.Employee_Username);
            return View(invoice);
        }

        [Authorize(Roles = "Admin")]
        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Employee_UsernameNavigation)
                .FirstOrDefaultAsync(m => m.Invoice_No == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        [Authorize(Roles = "Admin")]
        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'PharmcySystemContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return (_context.Invoices?.Any(e => e.Invoice_No == id)).GetValueOrDefault();
        }
    }
}
