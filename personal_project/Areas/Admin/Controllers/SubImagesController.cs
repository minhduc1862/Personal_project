using Microsoft.AspNetCore.Mvc;
using personal_project.Models;
using X.PagedList;

namespace personal_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubImagesController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            //số bản ghi trên một trang
            int pageSize = 4;
            //số trang
            int pageNumber = page ?? 1;
            var list_record = db.Products.Select(x => new { x.Id, x.Name, x.Photo}).OrderByDescending(item => item.Id).ToList();
            return View("Index", list_record.ToPagedList(pageNumber, pageSize));
        }
        //add image
        [HttpPost]
        public IActionResult Add(int id)
        {
            foreach (var file in Request.Form.Files)
            {
                if (file.Length > 0)
                {
                    ItemImage record = new ItemImage();
                    var _Photo = DateTime.Now.ToFileTime() + "_" + file.FileName;
                    var _Path = Path.Combine("wwwroot/Upload/Sub-images/", _Photo);
                    using (var stream = new FileStream(_Path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    record.ProductId = id;
                    record.Photo = _Photo;
                    db.Images.Add(record);
                }
                db.SaveChanges();
            }
            return Redirect("/Admin/SubImages/Index");
        }
        public IActionResult Delete(int id)
        {
            ItemImage record = db.Images.Where(item => item.Id == id).FirstOrDefault();
            if (record != null)
            {
                db.Images.Remove(record);
                db.SaveChanges();
            }
            return Redirect("/Admin/SubImages/Index");
        }
    }
}
