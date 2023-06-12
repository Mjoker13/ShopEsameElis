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
    public class ProductsController : Controller
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shopContext = _context.Products.Include(p => p.IdShopNavigation);
            return View(await shopContext.ToListAsync());
        }


      
        public IActionResult Create()
        {
            ViewData["IdShop"] = new SelectList(_context.Shops, "IdShop", "IdShop");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProduct,Name,Price,Quantity,IdShop")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdShop"] = new SelectList(_context.Shops, "IdShop", "IdShop", product.IdShop);
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["IdShop"] = new SelectList(_context.Shops, "IdShop", "IdShop", product.IdShop);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProduct,Name,Price,Quantity,IdShop")] Product product)
        {
            if (id != product.IdProduct)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.IdProduct))
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
            ViewData["IdShop"] = new SelectList(_context.Shops, "IdShop", "IdShop", product.IdShop);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.IdShopNavigation)
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShopContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.IdProduct == id)).GetValueOrDefault();
        }

        //public async Task<IActionResult> addToCart(int idProduct)
        //{
        //    _context.Products.Find(idProduct);
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> addToCart(Product product, Utente utente)
        //{
        //    var addCart = new Cart();
        //    addCart.IdProduct = product.IdProduct;
        //    addCart.IdUtente = utente.IdUtente;
        //    return RedirectToAction("CartsController.index");
        //}
    }
}
