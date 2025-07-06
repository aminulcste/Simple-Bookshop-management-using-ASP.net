using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookshopManagement.Models;
using BookshopManagement.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookshopManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            // Return the Register view without user navbar
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));

                    if (!await _roleManager.RoleExistsAsync("User"))
                        await _roleManager.CreateAsync(new IdentityRole("User"));

                    var totalUsers = await _userManager.Users.CountAsync();
                    if (totalUsers == 1)
                        await _userManager.AddToRoleAsync(user, "Admin");
                    else
                        await _userManager.AddToRoleAsync(user, "User");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // After registration redirect based on role
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("Index", "Admin");
                    else
                        return RedirectToAction("Index", "UserBooks");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Return login view without user navbar
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(
                            user.UserName,
                            model.Password,
                            model.RememberMe,
                            lockoutOnFailure: false);

                        if (result.Succeeded)
                        {
                            // Redirect based on role
                            if (await _userManager.IsInRoleAsync(user, "Admin"))
                                return RedirectToAction("Index", "Admin");
                            else
                                return RedirectToAction("Index", "UserBooks");
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email and Password are required.");
                }
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
