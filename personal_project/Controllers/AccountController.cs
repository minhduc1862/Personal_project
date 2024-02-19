using Microsoft.AspNetCore.Mvc;
using personal_project.Models;
using BC = BCrypt.Net.BCrypt;

namespace personal_project.Controllers
{
    public class AccountController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            return View();
        }
        // đăng nhập
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPost(IFormCollection fc)
        {
            string _Email = fc["Email"].ToString().Trim();
            string _Password = fc["Password"].ToString();
            var record = db.Customers.Where(item=>item.Email == _Email).FirstOrDefault();
            if (record != null)
            {
                if (BC.Verify(_Password, record.Password))
                {
                    HttpContext.Session.SetString("customer_id", record.Id.ToString());
                    HttpContext.Session.SetString("customer_email", _Email);
                    return Redirect("/Home");
                }
            }
            else
                return Redirect("/Account/Login?notify=invalid");
            return Redirect("/Account/Login?notify=valid");
        }
        // đăng kí
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult RegisterPost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Email = fc["Email"].ToString().Trim();
            string _Phone = fc["Phone"].ToString().Trim();
            string _Address = fc["Address"].ToString().Trim();
            string _Password = fc["Password"].ToString();
            _Password = BC.HashPassword(_Password);
            ItemCustomer record_check = db.Customers.FirstOrDefault(item => item.Email == _Email);
            if (record_check == null)
            {
                ItemCustomer record = new ItemCustomer();
                record.Name = _Name;
                record.Email = _Email;
                record.Phone = _Phone;
                record.Address = _Address;
                record.Password = _Password;
                db.Customers.Add(record);
                db.SaveChanges();
            }
            else
                return Redirect("/Account/Register?notify=email-exists");
            return Redirect("/Account/Register?notify=success");
        }
        // đăng xuất
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("customer_email");
            HttpContext.Session.Remove("customer_id");
            return Redirect("/Home");
        }
    }
}
