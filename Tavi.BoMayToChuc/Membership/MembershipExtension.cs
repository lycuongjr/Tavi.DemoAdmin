using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Tavi.BoMayToChuc.HtmlExtension;
using Tavi.BoMayToChuc.ModelEx;
using Tavi.BoMayToChuc.Service;

namespace Tavi.BoMayToChuc.Membership
{
    public static class MembershipExtension
    {
        public static ThongTinCanBo GetUserLogin(this IIdentity identity)
        {
            HttpSessionState session = HttpContext.Current.Session;
            var canbo = new ThongTinCanBo();
            if (session != null)
            {
                if (session["_USERID"] != null)
                    canbo.CanBoId = Convert.ToInt32(session["_USERID"]);
                else
                {
                    if (!HttpContext.Current.Response.IsRequestBeingRedirected) {
                        HttpContext.Current.Response.Redirect("~/Auth/Login");
                    }
                }
                if (session["_USERNAME"] != null)
                    canbo.TenDangNhap = session["_USERNAME"].ToString();
                if (session["_FULLNAME"] != null)
                    canbo.HoVaTen = session["_FULLNAME"].ToString();
                if (session["_DEPARTMENTID"] != null)
                    canbo.PhongBanHienTaiId = Convert.ToInt32(session["_DEPARTMENTID"]);
                if (session["_DEPARTMENTNAME"] != null)
                    canbo.TenPhongBan = session["_DEPARTMENTNAME"].ToString();
                if (session["_POSITIONID"] != null)
                    canbo.ChucVuHienTaiId = Convert.ToInt32(session["_POSITIONID"].ToString());
                if (session["_POSITIONNAME"] != null)
                    canbo.TenChucVu = session["_POSITIONNAME"].ToString();
                if (session["_UNITID"] != null)
                    canbo.CoQuanHienTaiId = Convert.ToInt32(session["_UNITID"].ToString());
                if (session["_UNITNAME"] != null)
                    canbo.TenCoQuan = session["_UNITNAME"].ToString();
                if (session["_EMAIL"] != null)
                    canbo.Email = session["_EMAIL"].ToString();
                if (session["_TEL"] != null)
                    canbo.DienThoai = session["_TEL"].ToString();
                if (session["_AVATAR"] != null)
                    canbo.AnhDaiDien = session["_AVATAR"].ToString();
                if (session["_ROLES"] != null)
                    canbo.DsChucNang = (List<string>)session["_ROLES"];
                return canbo;
            }
            else
            {
                //FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("~/Auth/Login", true);
                return canbo;
            }
        }

        public static void SetUserLogin(this IIdentity identity, ThongTinCanBo userInfo, List<string> lstRole)
        {

            HttpSessionState session = HttpContext.Current.Session;
            session.Add("_USERID", userInfo.CanBoId.ToString());
            session.Add("_USERNAME", userInfo.TenDangNhap);
            session.Add("_FULLNAME", userInfo.HoVaTen);
            session.Add("_DEPARTMENTID", userInfo.PhongBanHienTaiId.ToString());
            session.Add("_DEPARTMENTNAME", userInfo.TenPhongBan);
            session.Add("_POSITIONID", userInfo.ChucVuHienTaiId);
            session.Add("_POSITIONNAME", userInfo.TenChucVu);
            session.Add("_UNITID", userInfo.CoQuanHienTaiId.ToString());
            session.Add("_UNITNAME", userInfo.TenCoQuan.ToString());
            session.Add("_EMAIL", string.IsNullOrEmpty(userInfo.Email) ? "" : userInfo.Email.ToString());
            session.Add("_TEL", string.IsNullOrEmpty(userInfo.DienThoai) ? "" : userInfo.DienThoai.ToString());
            session.Add("_AVATAR", string.IsNullOrEmpty(userInfo.AnhDaiDien) ? "" : userInfo.AnhDaiDien.ToString());
            session.Add("_ROLES", lstRole);
            session.Timeout = 7200;
        }

        /// <summary>
        /// Login system by user info
        /// </summary>
        /// <param name="user">User info</param>
        /// <returns>Url to access</returns>
        public static string LoginByUser(ThongTinCanBo user, string reUrl = "")
        {
            ChucNangUngDungService _role = new ChucNangUngDungService();
            List<string> roles = _role.ChucNangSuDung(user.CanBoId).ToList();
            HttpContext.Current.User.Identity.SetUserLogin(user, roles);
            string url = String.Empty;
            if (string.IsNullOrEmpty(reUrl))
            {
                url = MenuHelper.GetFirstPermissionUrlByUser(user);
            }
            else url = reUrl;
            return url;
        }

        public static bool CheckUserRole(string roles)
        {
            if (string.IsNullOrEmpty(roles))
                return true;

            List<string> UserRoles = HttpContext.Current.User.Identity.GetUserLogin().DsChucNang;
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
