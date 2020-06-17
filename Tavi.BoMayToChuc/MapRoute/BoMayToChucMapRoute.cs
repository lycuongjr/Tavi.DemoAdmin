using System.Web.Mvc;

namespace Tavi.BoMayToChuc
{
    public class BoMayToChucAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BoMayToChuc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BoMayToChuc_default",
                "BoMayToChuc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Tavi.BoMayToChuc.Controllers" }
            );
        }
    }
}
