using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookshopManagement.Data;
using BookshopManagement.Models;
using System.Threading.Tasks;
using System.Linq;

namespace BookshopManagement.Controllers
{
    public class UserBooksController : Controller
    {
        private readonly AppDbContext _context;

        public UserBooksController(AppDbContext context)
        {
            _context = context;
        }

        // For all actions, load categories to ViewBag for layout dropdown
        private async Task LoadCategoriesAsync()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
        }

        // Home / All books
        public async Task<IActionResult> Index()
        {
            await LoadCategoriesAsync();

            var books = await _context.Books.Include(b => b.Category).ToListAsync();
            return View(books);
        }

        // Show books by category
        public async Task<IActionResult> Category(int id)
        {
            await LoadCategoriesAsync();

            var books = await _context.Books
                .Include(b => b.Category)
                .Where(b => b.CategoryId == id)
                .ToListAsync();

            var category = await _context.Categories.FindAsync(id);
            ViewData["CategoryName"] = category?.Name ?? "Category";

            return View("Index", books);
        }

        // Book details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            await LoadCategoriesAsync();

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return NotFound();

            return View(book);
        }
    }
}
