using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class AjaxController : Controller
    {
        private readonly NorthwindContext _context;

        public AjaxController(NorthwindContext context)
        {
            _context = context;
        }
        //GET: Ajax/Greet
        [HttpGet]
        public string Greet(string Name)
        {
            Thread.Sleep(3000);
            return $"Hello, {Name}!";
        }
        //POST: Ajax/PostGreet
        [HttpPost]
        public string PostGreet(string Name)
        {
            return $"Hello, {Name}!";
        }
        public IActionResult Index()
        {
            return View();
        }
        //POST: Ajax/CheckName
        [HttpPost]
        public string CheckName(string ContactName)
        {
            bool Exists = _context.Customers.Any(e => e.ContactName == ContactName);
            return Exists ? "true" : "false";
        }
    }
}
