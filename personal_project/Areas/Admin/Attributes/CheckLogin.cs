using Microsoft.AspNetCore.Mvc.Filters;
namespace personal_project.Areas.Admin.Attributes
{
    public class CheckLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string username = context.HttpContext.Session.GetString("admin_username");
            if (string.IsNullOrEmpty(username))
            {
                context.HttpContext.Response.Redirect("/Admin/Account/Login");
            }
            base.OnActionExecuting(context);
        }
    }
}
