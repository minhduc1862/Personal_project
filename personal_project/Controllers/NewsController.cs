using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using personal_project.Models;
using X.PagedList;

namespace personal_project.Controllers
{
    public class NewsController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            //số bản ghi trên một trang
            int pageSize = 9;
            //số trang
            int pageNumber = page ?? 1;
            List<ItemNews> list_record = db.News.OrderByDescending(item => item.Id).ToList();
            return View("Index", list_record.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Detail(int id)
        {
            var record = db.News.Where(item => item.Id == id).FirstOrDefault();
            return View("NewsDetail", record);
        }
    }
}
