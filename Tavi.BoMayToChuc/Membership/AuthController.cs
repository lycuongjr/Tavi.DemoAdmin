using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tavi.BoMayToChuc.HtmlExtension;
using Tavi.BoMayToChuc.ModelEx;
using Tavi.BoMayToChuc.Service;
using Tavi.Core.Membership;
using Tavi.Core.UI;

namespace Tavi.BoMayToChuc.Membership
{
    public class AuthController: Controller
    {
        [HttpGet]
        public ActionResult Login(string reUrl)
        {
            ViewBag.reUrl = reUrl;
            ThongTinCanBo userCookie = checkCookie();
            if (userCookie != null)
            {
                CanBoService _user = new CanBoService();
                ThongTinCanBo user = _user.GetUserLogin(userCookie.TenDangNhap, userCookie.MatKhau);
                if (user != null)
                {
                    // login system
                    string url = MembershipExtension.LoginByUser(user, reUrl);

                    if (!string.IsNullOrEmpty(url))
                    {
                        return Redirect(url);
                    }
                }
            }
            return View("Login2");
        }

        [HttpPost]
        public ActionResult Login(string txtUserName, string txtPassword, bool? Remember, string reUrl)
        {
            if (string.IsNullOrEmpty(txtUserName) || string.IsNullOrEmpty(txtPassword))
            {
                ModelState.AddModelError("", "Thiếu thông tin tài khoản hoặc mặt khẩu");
                return View();
            }

            CanBoService _user = new CanBoService();
            ThongTinCanBo user = _user.GetUserLogin(txtUserName, EncryptHelper.EncryptMD5(txtPassword));
            if (user != null)
            {
                // save username and password into cookie
                if (Remember != null && Remember == true)
                {
                    setCookie(txtUserName, EncryptHelper.EncryptMD5(txtPassword));
                }

                // login system
                string url = MembershipExtension.LoginByUser(user, reUrl);
                
                if (!string.IsNullOrEmpty(url))
                {
                    return Redirect(url);
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản chưa được phân quyền");
                }
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View("Login2");
        }

        public ThongTinCanBo checkCookie()
        {
            ThongTinCanBo user = null;
            string userName = string.Empty, passWord = string.Empty;
            if (Request.Cookies["username"] != null)
                userName = Request.Cookies["username"].Value;
            if (Request.Cookies["password"] != null)
                passWord = Request.Cookies["password"].Value;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
                user = new ThongTinCanBo { TenDangNhap = userName, MatKhau = passWord };
            return user;
        }

        public void setCookie(string userName, string passWord)
        {
            HttpCookie ckUsername = new HttpCookie("username");
            ckUsername.Expires = DateTime.Now.AddSeconds(3600);
            ckUsername.Value = userName;
            Response.Cookies.Add(ckUsername);
            HttpCookie ckPassword = new HttpCookie("password");
            ckPassword.Expires = DateTime.Now.AddSeconds(3600);
            ckPassword.Value = passWord;
            Response.Cookies.Add(ckPassword);
        }

        public ViewResult AccessDenied()
        {
            return View();
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["username"] != null)
            {
                HttpCookie ckUsername = new HttpCookie("username");
                ckUsername.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckUsername);
            }
            if (Request.Cookies["username"] != null)
            {
                HttpCookie ckPassword = new HttpCookie("password");
                ckPassword.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(ckPassword);
            }
            Session["_USERID"] = null;
            Session["_USERNAME"] = null;
            return RedirectToAction("Login", "Auth");
        }
    }
}
