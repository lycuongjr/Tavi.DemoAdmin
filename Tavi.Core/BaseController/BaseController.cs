using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tavi.Core.BaseController
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Render Razor view to string (JSON)
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.HttpContext.Request.Url.AbsoluteUri;
            if (Session["_USERID"] == null)
                filterContext.Result = RedirectToAction("Login", "Auth", new { area = "", reUrl = url });
        }


    }
}
