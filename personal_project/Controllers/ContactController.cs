using Microsoft.AspNetCore.Mvc;

namespace personal_project.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
