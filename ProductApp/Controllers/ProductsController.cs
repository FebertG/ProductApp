using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductApp.Data;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    /// <summary>
    /// Controller obsługujący operacje CRUD na encji Product w lokalnej bazie SQLite.
    /// </summary>
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        /// <summary>
        /// Konstruktor, wstrzykuje kontekst bazy danych EF Core.
        /// </summary>
        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: Products
        /// Wyświetla listę wszystkich produktów.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Products.ToListAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas ładowania listy produktów: {ex.Message}");
                return StatusCode(500, "Wystąpił błąd podczas pobierania produktów.");
            }
        }

        /// <summary>
        /// GET: Products/Details/
        /// Wyświetla szczegóły pojedynczego produktu.
        /// </summary>
        /// <param name="id">Id produktu.</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania szczegółów produktu: {ex.Message}");
                return StatusCode(500, "Wystąpił błąd podczas przetwarzania żądania.");
            }
        }

        /// <summary>
        /// GET: Products/Create
        /// Wyświetla formularz tworzenia nowego produktu.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Products/Create
        /// Tworzy nowy produkt w bazie danych.
        /// </summary>
        /// <param name="product">Dane produktu pobrane z formularza.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas zapisu produktu: {ex.Message}");
                    ModelState.AddModelError("", "Wystąpił błąd podczas zapisu produktu do bazy.");
                }
            }
            // Jeśli walidacja się nie powiedzie lub wystąpi wyjątek, zwraca formularz
            return View(product);
        }

        /// <summary>
        /// GET: Products/Edit/
        /// Wyświetla formularz edycji istniejącego produktu.
        /// </summary>
        /// <param name="id">Id produktu.</param>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas ładowania produktu do edycji: {ex.Message}");
                return StatusCode(500, "Wystąpił błąd podczas pobierania danych produktu.");
            }
        }

        /// <summary>
        /// POST: Products/Edit/
        /// Zapisuje zmiany w edytowanym produkcie.
        /// </summary>
        /// <param name="id">Id produktu.</param>
        /// <param name="product">Zmienione dane produktu.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description")] Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas edycji produktu: {ex.Message}");
                    ModelState.AddModelError("", "Wystąpił błąd podczas zapisu zmian.");
                }
            }

            return View(product);
        }

        /// <summary>
        /// GET: Products/Delete/
        /// Wyświetla stronę potwierdzenia usunięcia produktu.
        /// </summary>
        /// <param name="id">Id produktu.</param>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas ładowania produktu do usunięcia: {ex.Message}");
                return StatusCode(500, "Wystąpił błąd podczas pobierania danych.");
            }
        }

        /// <summary>
        /// POST: Products/Delete/
        /// Usuwa produkt z bazy danych po potwierdzeniu.
        /// </summary>
        /// <param name="id">Id produktu do usunięcia.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas usuwania produktu: {ex.Message}");
                return StatusCode(500, "Wystąpił błąd podczas usuwania produktu.");
            }
        }

        /// <summary>
        /// Sprawdza, czy produkt o danym id istnieje w bazie.
        /// </summary>
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
