using Microsoft.AspNetCore.Mvc;
using BookshopManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace BookshopManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

       
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Welcome to Our Bookshop!";
            var books = await _context.Books.Include(b => b.Category).ToListAsync();
            return View(books);
        }
    }
}

