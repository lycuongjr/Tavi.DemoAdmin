using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tavi.BoMayToChuc.Membership;
using Tavi.BoMayToChuc.ModelEx;
using Tavi.Core.Membership;

namespace Tavi.BoMayToChuc.HtmlExtension
{
    public static class MenuHelper
    {
        #region menu left
        public static MvcHtmlString MenuLeft(this HtmlHelper helper, string app)
        {
            return GetTreeSiteMapMenuLeft(helper, app, SiteMap.RootNode);
        }
        private static MvcHtmlString GetTreeSiteMapMenuLeft(HtmlHelper helper, string app, SiteMapNode node)
        {
            return GetTreeSiteMapMenuLeft(helper, app, node, true);
        }
        private static MvcHtmlString GetTreeSiteMapMenuLeft(HtmlHelper helper, string app, SiteMapNode node, bool isRoot)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<nav class='mt - 2'>");
            sb.Append("<ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' id='tavimenu' role='menu'>");
            foreach (SiteMapNode childNode in node.ChildNodes)
            {
                if (CheckAccess(helper, childNode, app))
                {
                    string icon_expand = string.Empty;
                    string menuchild = SiteMapMenuChildLeft(helper, childNode, app);
                    string link = GetUrlSiteMapMenuChild(helper, childNode, app, false);
                    string istreeview = "";
                    if (childNode.ChildNodes.Count > 0 && menuchild != "")
                    {
                        link = "#";
                        istreeview = "has-treeview";
                    }

                    sb.AppendFormat("<li index='{0}' class='nav-item "+ istreeview + "'>", childNode["index"]);
                    sb.AppendLine("<a href='"+ link+"' class='nav-link'>");
                    if (childNode["icon"] != null && childNode["icon"] != "")
                        sb.AppendLine("<i class='nav-icon "+ childNode["icon"] + "'></i>");
                    else
                        sb.AppendLine("<i class='nav-icon fas fa-file'></i>");

                    sb.AppendLine("<p>" + helper.Encode(childNode.Title) + (childNode.HasChildNodes ? "<i class='fas fa-angle-left right'></i>" : "") + "</p>");
                    sb.AppendLine("</a>");
                   
                    sb.Append(menuchild);
                    sb.AppendLine("</li>");

                }

            }
            sb.Append("</ul>");
            sb.Append("</nav>");

            return new MvcHtmlString(sb.ToString());
        }

        private static string SiteMapMenuChildLeft(HtmlHelper helper, SiteMapNode node, string appcode)
        {
            if (node.ChildNodes.Count > 0)
            {
                bool check = false;
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul class='nav nav-treeview'>");
                foreach (SiteMapNode childNode in node.ChildNodes)
                {
                    if (CheckAccess(helper, childNode, appcode))
                    {
                        string target = string.Empty;
                        if (childNode["target"] != null && childNode["target"] != "")
                            target = "target= _blank";
                        if (childNode["number"] != null && childNode["number"] != "")
                        {
                            sb.AppendFormat("<li index='{0}'>", childNode["index"]);
                            if (childNode["icon"] != null && childNode["icon"] != "")
                            {
                                sb.AppendFormat("<a  class='nav-link' href='{0}' " + target + " ><i class='" + childNode["icon"] + "'> </i>   <p>{1}</p> <span class='badge badge-info right number'></span></a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            else
                            {
                                sb.AppendFormat("<a  class='nav-link' href='{0}' " + target + " ><i class='fas fa-caret-right nav-icon'> </i>   <p>{1}</p> <span class='badge badge-info right number'></span></a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            sb.AppendLine("</li>");
                            check = true;
                        }
                        else
                        {
                            sb.AppendFormat("<li index='{0}' class='nav-item'>", childNode["index"]);
                            if (childNode["icon"] != null && childNode["icon"] != "")
                            {
                                sb.AppendFormat("<a class='nav-link' href='{0}'  " + target + " ><i class='" + childNode["icon"] + "'> </i>  <p>{1}</p></a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            else
                            {
                                sb.AppendFormat("<a  class='nav-link' href='{0}'  " + target + " ><i class='fas fa-caret-right nav-icon'> </i>  <p>{1}</p></a>", childNode.Url, helper.Encode(childNode.Title));
                            }
                            sb.AppendLine("</li>");
                            check = true;
                        }

                    }
                }
                sb.AppendLine("</ul>");
                if (check)
                    return sb.ToString();
                else
                    return "";
            }

            return "";
        }
        private static bool CheckAccess(HtmlHelper helper, SiteMapNode node, string appcode)
        {
            if (node.Roles.Count == 0)
            {
                return true;
            }
            for (int i = 0; i < node.Roles.Count; i++)
            {
                if (node.Roles[i] != null)
                {
                    string role = node.Roles[i].ToString();
                    if (MembershipExtension.CheckUserRole(role))
                        return true;
                }
            }
            return false;

        }
        private static string GetUrlSiteMapMenuChild(HtmlHelper helper, SiteMapNode node, string appcode, bool isRoot)
        {
            foreach (SiteMapNode childNode in node.ChildNodes)
            {
                if (CheckAccess(helper, childNode, appcode))
                {
                    return childNode.Url.ToString();
                }
            }
            return node.Url.ToString();
        }
        public static string GetFirstPermissionUrlByUser(ThongTinCanBo user)
        {
            foreach (SiteMapNode node in SiteMap.RootNode.ChildNodes)
            {
                if(node.ChildNodes.Count == 0)
                {
                    if (CheckAccessByUser(node, user))
                    {
                        return node.Url;
                    }
                }
                else {
                    foreach (SiteMapNode childNode in node.ChildNodes)
                    {
                        if (CheckAccessByUser(childNode, user))
                        {
                            return childNode.Url;
                        }
                    }
                }
            }
            return string.Empty;
        }
        public static bool CheckAccessByUser(SiteMapNode node, ThongTinCanBo user)
        {
            if (user != null)
            {
                if (node.Roles.Count == 0)
                {
                    return true;
                }
                for (int i = 0; i < node.Roles.Count; i++)
                {
                    if (node.Roles[i] != null)
                    {
                        string role = node.Roles[i].ToString();
                        return MembershipExtension.CheckUserRole(role);
                    }
                }
            }
            return false;

        }
        #endregion
    }
}
