
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopEsameElis.Models;

namespace ShopEsameElis.Controllers
{
    public class UtentesController : Controller
    {
        private readonly ShopContext _context;

        public UtentesController(ShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Utentes != null ? 
                          View(await _context.Utentes.ToListAsync()) :
                          Problem("Entity set 'ShopContext.Utentes'  is null.");
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUtente,Name,Surname")] Utente utente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Utentes == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }
            return View(utente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUtente,Name,Surname")] Utente utente)
        {
            if (id != utente.IdUtente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtenteExists(utente.IdUtente))
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
            return View(utente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utentes == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes
                .FirstOrDefaultAsync(m => m.IdUtente == id);
            if (utente == null)
            {
                return NotFound();
            }

            return View(utente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utentes == null)
            {
                return Problem("Entity set 'ShopContext.Utentes'  is null.");
            }
            var utente = await _context.Utentes.FindAsync(id);
            if (utente != null)
            {
                _context.Utentes.Remove(utente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtenteExists(int id)
        {
          return (_context.Utentes?.Any(e => e.IdUtente == id)).GetValueOrDefault();
        }

        public Utente Login(int idUtente,string name, string surname)
        {
            var utente = _context.Utentes.Find(idUtente);
            
                if (utente.Name == name & utente.Surname == surname)
                {
                    return utente;
                }
                return null;
            
        }
    }
}
