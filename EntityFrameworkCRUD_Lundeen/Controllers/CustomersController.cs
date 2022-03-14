#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCRUD_Lundeen.Context;
using EntityFrameworkCRUD_Lundeen.Models;

namespace EntityFrameworkCRUD_Lundeen.Controllers
{
    public class CustomersController : Controller
    {
        private readonly SalesContext _context;

        public CustomersController(SalesContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> SearchHelper(string Phrase, string Type)
        {
            if (Phrase != null)
            {
                switch (Type)
                {
                    case "lastname":
                        return (_context.Customers
                            .Where(c => c.LastName.ToLower() == Phrase.ToLower()));

                    case "lastinitial":
                        return (_context.Customers
                            .Where(c => (c.LastName.ToLower().FirstOrDefault() == Phrase.ToLower().FirstOrDefault())));

                    case "city":
                        return (_context.Customers
                            .Where(c => c.City.ToLower() == Phrase.ToLower()));

                    default:
                        return _context.Customers.ToList();
                }
            }
            return _context.Customers.ToList();

        }

        // GET: Customers/fuller/lastname
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
            
        }

        [HttpPost]
        public IActionResult Index(string SearchPhrase, string type)
        {
            return View(SearchHelper(SearchPhrase, type));
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(include:"FirstName,LastName,City,Country,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.SaveCustomer(customer); // This is the only new line in this function
                
                //_context.Add(customer);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName,LastName,City,Country,Phone")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
