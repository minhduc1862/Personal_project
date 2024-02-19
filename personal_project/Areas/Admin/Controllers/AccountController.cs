using Microsoft.AspNetCore.Mvc;
using personal_project.Models;
using BC = BCrypt.Net.BCrypt;

namespace personal_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _username = fc["Username"].ToString().Trim();
            string _password = fc["Password"].ToString().Trim();
            var record = db.Users.Where(item => item.Username == _username).FirstOrDefault();
            if (record != null)
            {
                if (BC.Verify(_password, record.Password))
                {
                    HttpContext.Session.SetString("admin_user_id", record.Id.ToString());
                    HttpContext.Session.SetString("admin_username", _username);
                    return Redirect("/Admin/Home");
                }
            }
            return Redirect("/Admin/Account/Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin_user_id");
            HttpContext.Session.Remove("admin_username");
            return Redirect("/Admin/Account/Login");
        }
    }
}
