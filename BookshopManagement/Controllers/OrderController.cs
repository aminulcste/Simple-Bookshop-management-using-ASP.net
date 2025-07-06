using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BookshopManagement.Data;
using BookshopManagement.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace BookshopManagement.Controllers
{
    [AllowAnonymous] // Keep your existing allowance for public actions
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string SessionCartKey = "Cart";

        public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString(SessionCartKey);
            if (string.IsNullOrEmpty(sessionCart))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(sessionCart) ?? new List<CartItem>();
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove(SessionCartKey);
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Count == 0)
                return RedirectToAction("Index", "Cart");

            foreach (var item in cart)
            {
                item.Book = _context.Books.FirstOrDefault(b => b.BookId == item.BookId);
            }

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
        {
            var cart = GetCart();
            if (cart.Count == 0)
                return RedirectToAction("Index", "Cart");

            // You can assign a dummy user or skip user assignment completely
            var order = new Order
            {
                UserId = null, // or some default value or get current user id if logged in
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cart)
            {
                var price = _context.Books
                    .Where(b => b.BookId == item.BookId)
                    .Select(b => b.Price)
                    .FirstOrDefault();

                order.OrderItems.Add(new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            ClearCart();

            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public async Task<IActionResult> MyOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .Where(o => o.UserId == null) // You might want to filter by current user id here
                .ToListAsync();

            return View(orders);
        }

        // *** New AdminOrders action to display all user orders ***
        [Authorize(Roles = "Admin")] // Optional: restrict to Admin role only
        public async Task<IActionResult> AdminOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: Order/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminOrders));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }


    }
}
