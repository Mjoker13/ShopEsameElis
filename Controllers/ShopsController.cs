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
    public class ShopsController : Controller
    {
        private readonly ShopContext _context;

        public ShopsController(ShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Shops != null ? 
                          View(await _context.Shops.ToListAsync()) :
                          Problem("Entity set 'ShopContext.Shops'  is null.");
        }

       

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdShop,Name")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdShop,Name")] Shop shop)
        {
            if (id != shop.IdShop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.IdShop))
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
            return View(shop);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .FirstOrDefaultAsync(m => m.IdShop == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shops == null)
            {
                return Problem("Entity set 'ShopContext.Shops'  is null.");
            }
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopExists(int id)
        {
          return (_context.Shops?.Any(e => e.IdShop == id)).GetValueOrDefault();
        }
    }
}
