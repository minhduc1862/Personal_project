using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using person_projectAPI.Data;

namespace person_projectAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetDiscountProducts()
        {
            var listProduct = _context.Products.Where(item => item.Discount > 0).OrderByDescending(item => item.Id).Take(8).ToList();
            if (listProduct.Count > 0)
            {
                return Ok(listProduct);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult GetHotProducts()
        {
            var listProduct = _context.Products.Where(item => item.Hot == 1).OrderByDescending(item => item.Id).Take(8).ToList();
            if (listProduct.Count > 0)
            {
                return Ok(listProduct);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult GetHotNews()
        {
            var listNew = _context.News.Where(item => item.Hot == 1).OrderByDescending(item => item.Id).Take(3).ToList();
            if (listNew.Count > 0)
            {
                return Ok(listNew);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult GetSlideShow()
        {
            var listSlide = _context.Slides.OrderByDescending(item => item.Id).ToList();
            if (listSlide.Count > 0)
            {
                return Ok(listSlide);
            }
            return NotFound();
        }
    }
}
