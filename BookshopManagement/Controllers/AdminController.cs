using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookshopManagement.Controllers
{
  
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View(); // You need to create Views/Admin/Index.cshtml
        }
    }
}
