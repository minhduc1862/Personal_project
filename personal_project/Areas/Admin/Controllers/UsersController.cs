using Microsoft.AspNetCore.Mvc;
using personal_project.Models;
using BC = BCrypt.Net.BCrypt;
using X.PagedList;

namespace personal_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index()
        {
            return Redirect("/Admin/Users/Read");
        }

        public IActionResult Read(int? page)
        {
            int pageSize = 2; //bao nhiêu bản ghi trên 1 trang
            int pageNumber = page ?? 1;
            List<ItemUser> users = db.Users.OrderByDescending(item=>item.Id).ToList();
            return View("Read", users.ToPagedList(pageNumber, pageSize));
        }

        //update
        public IActionResult Update(int id)
        {
            ViewBag.action = "/Admin/Users/UpdatePost/" + id;
            ItemUser record = db.Users.FirstOrDefault(item => item.Id == id);
            return View("CreateUpdate", record);
        }

        [HttpPost]
        public IActionResult UpdatePost(int id, IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Username = fc["Username"].ToString().Trim();
            string _Password = fc["Password"].ToString();
            // lấy 1 bản ghi tương ứng với id truyền vào
            ItemUser record = db.Users.FirstOrDefault(item => item.Id == id);
            if (record != null)
            {
                //kiểm tra xem email này đã tồn tại trong table Users chưa, nếu chưa thì mới cho update
                ItemUser record_check = db.Users.FirstOrDefault(item => item.Id != id && item.Username == _Username);
                if (record_check == null)
                {
                    record.Name = _Name;
                    record.Username = _Username;
                    if (!String.IsNullOrEmpty(_Password))
                    {
                        //mã hoá password
                        _Password = BC.HashPassword(_Password);
                        record.Password = _Password;
                    }
                    // cập nhật lại dữ liệu
                    db.Update(record);
                    db.SaveChanges();
                }
                else
                    return Redirect("/Admin/Users/Update/" + id + "?notify=email-exists");
            }
            return Redirect("/Admin/Users/Read");
        }

        //create
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Users/CreatePost";
            return View("CreateUpdate");
        }
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            string _Username = fc["Username"].ToString().Trim();
            string _Password = fc["Email"].ToString();
            // mã hoá password
            _Password = BC.HashPassword(_Password);
            // kiểm tra trong csdl xem đã tồn tại email chưa nếu chưa thì mới update
            ItemUser record_check = db.Users.FirstOrDefault(item => item.Username == _Username);
            if (record_check == null)
            {
                ItemUser record = new ItemUser();
                record.Name = _Name;
                record.Username = _Username;
                record.Password = _Password;
                // cập nhật lại dữ liệu
                db.Users.Add(record);
                db.SaveChanges();
            }
            else
                return Redirect("/Admin/Users/Create?notify=email-exists");
            return Redirect("/Admin/Users/Read");
        }

        //delete
        public IActionResult Delete(int id)
        {
            // lấy một bản ghi tương ứng với id truyền vào
            ItemUser record = db.Users.FirstOrDefault(item => item.Id == id);
            // xoá bản ghi
            if (record != null)
            {
                db.Users.Remove(record);
                db.SaveChanges();
            }
            return Redirect("/Admin/Users/Read");
        }
    }
}
