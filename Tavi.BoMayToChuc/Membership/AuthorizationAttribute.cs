using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tavi.BoMayToChuc.Membership;

namespace Tavi.Core.Membership
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public string Role { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!CheckUserRole(filterContext, Role))
            {
                filterContext.Result = new RedirectResult("/Auth/AccessDenied");
            }
        }

        protected bool CheckUserRole(AuthorizationContext context, string roles)
        {
            if (string.IsNullOrEmpty(roles))
                return true;

            List<string> UserRoles = context.HttpContext.User.Identity.GetUserLogin().DsChucNang;
            List<string> lstRole = roles.Split(',').ToList();
            foreach (string role in lstRole)
            {
                if (UserRoles.Contains(role))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
