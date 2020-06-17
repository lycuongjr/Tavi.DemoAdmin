using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tavi.Core.UI;

namespace Tavi.Core.BaseController
{
    public class ExtensionController : BaseController
    {
        public ActionResult FileManager()
        {
            return View();
        }

        public PartialViewResult GetListFile()
        {
            FileInfo[] files = FileHelper.GetFilesFromDirectory();
            var result = new List<FileInfo>();
            if (files != null && files.Count() > 0)
            {
                result = files.ToList();
            }
            return PartialView("ListFileUploaded", result);
        }

        public JsonResult RemoveFile(string filePath)
        {
            filePath = HttpUtility.UrlDecode(filePath);
            var result = FileHelper.RemoveFile(filePath);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveFile()
        {
            var result = FileHelper.UploadFile(Request.Files);
//<<<<<<< HEAD
            return Json(!string.IsNullOrEmpty(result), JsonRequestBehavior.AllowGet);
//=======
//            return Json(result, JsonRequestBehavior.AllowGet);
//>>>>>>> LuongVX/dev
        }

        public JsonResult SetCollapseMenu(bool isCollapse)
        {
            if (Session["isCollapseMenu"] == null)
            {
                Session.Add("isCollapseMenu", isCollapse);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            Session.Remove("isCollapseMenu");
            Session.Add("isCollapseMenu", isCollapse);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
