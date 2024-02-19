using Microsoft.AspNetCore.Mvc;
using personal_project.Models;
using X.PagedList;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using personal_project.Areas.Admin.Attributes;

namespace personal_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class CategoriesController : Controller
    {
        string strConnectionString;
        public CategoriesController()
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            this.strConnectionString = config.GetConnectionString("MyConnectionString").ToString();
        }
        public IActionResult Index()
        {
            return Redirect("/Admin/Categories/Read");
        }

        public IActionResult Read(int? page)
        {
            DataTable dtCategories = new DataTable();
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Categories where ParentId = 0 order by Id desc", conn);
                da.Fill(dtCategories);
            }
            //---
            List<ItemCategory> listCategory = new List<ItemCategory>();
            foreach (DataRow item in dtCategories.Rows)
            {
                listCategory.Add(new ItemCategory() { Id = Convert.ToInt32(item["id"]), ParentId = Convert.ToInt32(item["ParentId"]), Name = item["Name"].ToString(), DisplayHomePage = Convert.ToInt32(item["DisplayHomePage"]) });
            }
            //---
            int pageSize = 4;
            int pageNumber = page ?? 1;
            //---
            return View("Read", listCategory.ToPagedList(pageNumber, pageSize));
        }

        //update
        public IActionResult Update(int id)
        {
            ViewBag.action = "/Admin/Categories/UpdatePost/" + id;
            DataTable dtCategory = new DataTable();
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Categories where Id = " + id, conn);
                da.Fill(dtCategory);
            }
            return View("CreateUpdate", dtCategory);
        }

        // update - sau khi ấn nút submit
        [HttpPost]
        public IActionResult UpdatePost(int id, IFormCollection fc)
        {
            // sử dụng đối tượng IFormCollection để lấy dữ liệu theo kiểu POST
            string _Name = fc["Name"].ToString().Trim();
            // sử dụng đối tượng Request để lấy dữ liệu theo kiểu POST
            int _ParentId = Convert.ToInt32(Request.Form["ParentId"]);
            int _DisplayHomePage = !String.IsNullOrEmpty(Request.Form["DisplayHomePage"]) ? 1 : 0;
            //---
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Update Categories set Name=@var_name, ParentId = @var_parent_id, DisplayHomePage = @display_home_page where Id = @id", conn);
                cmd.Parameters.AddWithValue("@var_name", _Name);
                cmd.Parameters.AddWithValue("@var_parent_id", _ParentId);
                cmd.Parameters.AddWithValue("@display_home_page", _DisplayHomePage);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            //---
            return Redirect("/Admin/Categories/Read");
        }

        //create
        public IActionResult Create()
        {
            ViewBag.action = "/Admin/Categories/CreatePost";
            return View("CreateUpdate");
        }
        public IActionResult CreatePost(IFormCollection fc)
        {
            string _Name = fc["Name"].ToString().Trim();
            int _ParentId = Convert.ToInt32(Request.Form["ParentId"]);
            int _DisplayHomePage = !String.IsNullOrEmpty(Request.Form["DisplayHomePage"]) ? 1 : 0;
            //---
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into Categories(Name,ParentId,DisplayHomePage) values(@var_name,@var_parent_id,@display_home_page)", conn);
                cmd.Parameters.AddWithValue("@var_name", _Name);
                cmd.Parameters.AddWithValue("@var_parent_id", _ParentId);
                cmd.Parameters.AddWithValue("@display_home_page", _DisplayHomePage);
                cmd.ExecuteNonQuery();
            }
            //---
            return Redirect("/Admin/Categories/Read");
        }

        //delete
        public IActionResult Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(strConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Delete from Categories where Id = @id or ParentId = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return Redirect("/Admin/Categories/Read");
        }
    }
}
