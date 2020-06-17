using System.Web.Mvc;

namespace Tavi.MRS
{
    public class BoMayToChucAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MRS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MRS_default",
                "MRS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Tavi.MRS.Controllers" }
            );
        }
    }
}
