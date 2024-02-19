using Microsoft.AspNetCore.Mvc;
using personal_project.Models;

namespace personal_project.Controllers
{
    public class HomeController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            return View();
        }
    }
}
