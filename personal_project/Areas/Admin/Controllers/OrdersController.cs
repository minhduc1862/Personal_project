using Microsoft.AspNetCore.Mvc;
using personal_project.Areas.Admin.Attributes;
using personal_project.Models;
using X.PagedList;

namespace personal_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class OrdersController : Controller
    {
        public MyDbContext db = new MyDbContext();
        public IActionResult Index(int? page)
        {
            int _CurrentPage = page ?? 1;
            int _RecordPerPage = 20;
            List<ItemOrder> listRecord = db.Orders.OrderByDescending(item => item.Id).ToList();
            return View("Index", listRecord.ToPagedList(_CurrentPage, _RecordPerPage));
        }
        //chi tiet san pham
        public IActionResult Detail(int? id)
        {
            int _OrderId = id ?? 0;
            ViewBag.OrderId = _OrderId;
            List<ItemOrderDetail> _ListRecord = db.OrdersDetail.Where(tbl => tbl.OrderId == _OrderId).ToList();
            return View("Detail", _ListRecord);
        }
        //giao hang
        public IActionResult Delivery(int? id)
        {
            int _OrderId = id ?? 0;
            ItemOrder record = db.Orders.Where(tbl => tbl.Id == _OrderId).FirstOrDefault();
            if (record != null)
            {
                record.Status = 1;
                db.SaveChanges();
            }
            var result = (from item_order in db.Orders
                          join item_orderdetail in db.OrdersDetail
                          on item_order.Id equals item_orderdetail.OrderId
                          where item_orderdetail.OrderId == id && item_order.Status == 1
                          select item_orderdetail).ToList();
            foreach (var item in result)
            {
                ItemProduct itemProduct = db.Products.Where(tbl => tbl.Id == item.ProductId).FirstOrDefault();
                if (itemProduct != null)
                {
                    itemProduct.Stock -= item.Quantity;
                    db.SaveChanges();
                }
            }
            return Redirect("/Admin/Orders");
        }
    }
}
