using Microsoft.AspNetCore.Mvc;
using BookshopManagement.Data;
using BookshopManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace BookshopManagement.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private const string SessionCartKey = "Cart";

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Safely get the cart from session
        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString(SessionCartKey);
            if (string.IsNullOrEmpty(sessionCart))
                return [];

            return JsonSerializer.Deserialize<List<CartItem>>(sessionCart) ?? [];
        }

        // Save the cart to session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(SessionCartKey, JsonSerializer.Serialize(cart));
        }

        // Display the cart
        public IActionResult Index()
        {
            var cart = GetCart();

            // Load book details from DB with null check
            foreach (var item in cart)
            {
                var book = _context.Books.FirstOrDefault(b => b.BookId == item.BookId);
                if (book is not null)
                {
                    item.Book = book;
                }
            }

            return View(cart);
        }

        // Add book to cart
        public IActionResult AddToCart(int id)
        {
            var book = _context.Books.Find(id);
            if (book is null)
                return NotFound();

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.BookId == id);

            if (cartItem is not null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem { BookId = id, Quantity = 1 });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // Remove book from cart
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.BookId == id);

            if (cartItem is not null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }
    }
}
