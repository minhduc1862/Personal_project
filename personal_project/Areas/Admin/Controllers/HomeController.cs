using Microsoft.AspNetCore.Mvc;
using personal_project.Areas.Admin.Attributes;

namespace personal_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
