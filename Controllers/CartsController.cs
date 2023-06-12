using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopEsameElis.Models;

namespace ShopEsameElis.Controllers
{
    public class CartsController : Controller
    {
        private readonly ShopContext _context;

        public CartsController(ShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Carts.Include(c => c.IdProductNavigation).Include(c => c.IdUtenteNavigation);
            return View(await shopContext.ToListAsync());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", cart.IdProduct);
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "IdUtente", cart.IdUtente);
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCart,IdUtente,IdProduct")] Cart cart)
        {
            if (id != cart.IdCart)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.IdCart))
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
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", cart.IdProduct);
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "IdUtente", cart.IdUtente);
            return View(cart);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.IdProductNavigation)
                .Include(c => c.IdUtenteNavigation)
                .FirstOrDefaultAsync(m => m.IdCart == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'ShopContext.Carts'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.Carts?.Any(e => e.IdCart == id)).GetValueOrDefault();
        }

        public IActionResult Create()
        {
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "Name");
            ViewData["Quantity"] = new SelectList(_context.Products, "IdProduct", "Quantity");
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCart,IdUtente,IdProduct")] Cart cart, int quantity)
        {

            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", cart.IdProduct);
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "IdUtente", cart.IdUtente);
            return View(cart);
        }

        //public IActionResult cart()
        //{
        //    var query = from p in _context.Products
        //                join c in _context.Carts
        //                on p.IdProduct equals c.IdProduct
        //                join u in _context.Utentes
        //                on c.IdUtente equals u.IdUtente
        //                select new
        //                {
        //                    u.Name,
        //                    //p.Name
        //                };
        //    return View(query.ToList());
        //}
    }
}
