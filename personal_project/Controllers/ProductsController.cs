using Microsoft.AspNetCore.Mvc;
using personal_project.Models;
using X.PagedList;

namespace personal_project.Controllers
{
    public class ProductsController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Categories(int? id, int? page)
        {
            int CategoryId = id ?? 0;
            ViewBag.CategoryId = CategoryId;
            //số bản ghi trên một trang
            int pageSize = 9;
            //số trang
            int pageNumber = page ?? 1;
            List<ItemProduct> list_record = new List<ItemProduct>();
            if (CategoryId > 0)
                //lấy các sản phẩm tương ứng với CategoryId truyền từ url
                list_record = (from category in db.Categories
                               join category_product in db.CategoriesProducts
                               on category.Id equals category_product.CategoryId
                               join product in db.Products
                               on category_product.ProductId equals product.Id
                               where category_product.CategoryId == CategoryId
                               orderby product.Id descending
                               select product).ToList();
            else
                list_record = db.Products.OrderByDescending(item => item.Id).ToList();
            return View("ProductsCategories", list_record.ToPagedList(pageNumber, pageSize));
        }
        public IActionResult Detail(int id)
        {
            var record = db.Products.Where(item => item.Id == id).FirstOrDefault();
            return View("ProductsDetail", record);
        }
        public IActionResult Rate(int id)
        {
            int star = Convert.ToInt32(Request.Query["star"]);
            // tạo đối tượng ItemRating để insert vào table Rating
            ItemRating record = new ItemRating();
            record.ProductId = id;
            record.Star = star;
            db.Rating.Add(record);
            db.SaveChanges();
            return Redirect("/Products/Detail/" + id);
        }
    }
}
