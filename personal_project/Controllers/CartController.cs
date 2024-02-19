using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using personal_project.Models;
using personal_project.Services;

namespace personal_project.Controllers
{
    public class CartController : Controller
    {
        private readonly PaypalClient _paypalClient;
        public CartController(PaypalClient paypalClient)
        {
            _paypalClient = paypalClient;
        }
        public IActionResult Index()
        {
            List<Item> shopping_cart = new List<Item>();
            string json_cart = HttpContext.Session.GetString("cart");
            if (!String.IsNullOrEmpty(json_cart))
            {
                shopping_cart = JsonConvert.DeserializeObject<List<Item>>(json_cart);
            }
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(shopping_cart);
        }
        // thêm sản phẩm vào giỏ hàng
        public IActionResult Buy(int id, int quantity = 1, string size = "")
        {
            Cart.CartAdd(HttpContext.Session, id, quantity, size);
            return RedirectToAction("Index");
        }
        // xoá sản phẩm khỏi giỏ hàng
        public IActionResult Remove(int id)
        {
            Cart.CartRemove(HttpContext.Session, id);
            return RedirectToAction("Index");
        }
        // update số lượng sản phẩm
        public IActionResult Update()
        {
            List<Item> shopping_cart = new List<Item>();
            string json_cart = HttpContext.Session.GetString("cart");
            if (!String.IsNullOrEmpty(json_cart))
            {
                shopping_cart = JsonConvert.DeserializeObject<List<Item>>(json_cart);
            }
            foreach (Item cart_item in shopping_cart)
            {
                int new_quantity = Convert.ToInt32(Request.Form["quantity_" + cart_item.ProductRecord.Id]);
                string new_size = Request.Form["size_" + cart_item.ProductRecord.Id];
                Cart.CartUpdate(HttpContext.Session, cart_item.ProductRecord.Id, new_quantity, new_size);
            }
            return RedirectToAction("Index");
        }
        // xoá toàn bộ sản phẩm
        public IActionResult Destroy()
        {
            Cart.CartDestroy(HttpContext.Session);
            return Redirect("/Cart/Index?notify=destroy-success");
        }
        // thanh toán giỏ hàng
        public IActionResult CheckOut()
        {
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("customer_id")))
            {
                return Redirect("/Account/Login");
            }
            else
            {
                int customer_id = Convert.ToInt32(HttpContext.Session.GetString("customer_id"));
                Cart.CartCheckOut(HttpContext.Session, customer_id);
            }
            return Redirect("/Cart/Index?notify=checkout-success");
        }

        #region Paypal payment
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            // Thông tin đơn hàng gửi qua Paypal
            var tongTien = (Cart.CartTotal(HttpContext.Session) + 2).ToString();
            var donviTienTe = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();
            
            try
            {
                var response = await _paypalClient.CreateOrder(tongTien, donviTienTe, maDonHangThamChieu);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                // Lưu database đơn hàng của mình

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        #endregion
    }
}
