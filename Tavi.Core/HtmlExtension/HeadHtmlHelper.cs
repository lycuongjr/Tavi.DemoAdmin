using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
namespace Tavi.Core.HtmlEx
{
    public static class HeadHtmlHelper
    {
        public static MvcHtmlString BaseUrl(this HtmlHelper helper)
        {
            return BaseUrl(helper, string.Empty);
        }
        public static MvcHtmlString BaseUrl(this HtmlHelper helper, string subPath)
        {
            HttpRequestBase Request = helper.ViewContext.RequestContext.HttpContext.Request;
            string BaseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            if (!string.IsNullOrEmpty(subPath))
            {
                BaseUrl += subPath.Trim('/');
            }
            return new MvcHtmlString(BaseUrl);
        }

        public static MvcHtmlString Css(this HtmlHelper helper)
        {
            return Css(helper, "css");
        }

        public static MvcHtmlString Css(this HtmlHelper helper, string cssFile)
        {
            return Css(helper, cssFile, "");
        }

        public static MvcHtmlString Css(this HtmlHelper helper, string cssFile, string htmlAttributes)
        {
            return Css(helper, cssFile, htmlAttributes, true);
        }

        public static MvcHtmlString Css(this HtmlHelper helper, string cssFile, string htmlAttributes, bool isRef)
        {
            string html = string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}{1}\" {2} />", BaseUrl(helper), cssFile.TrimStart('/'), htmlAttributes);
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString LinkIcon(this HtmlHelper helper, string iconFile)
        {
            string html = string.Format("<link rel=\"Shortcut Icon\" type=\"image/x-icon\" href=\"{0}{1}\" />", BaseUrl(helper), iconFile.TrimStart('/'));
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString Js(this HtmlHelper helper)
        {
            return Js(helper, "js");
        }

        public static MvcHtmlString Js(this HtmlHelper helper, string jsFile)
        {
            return Js(helper, jsFile, true);
        }

        public static MvcHtmlString Js(this HtmlHelper helper, string jsFile, bool isRef)
        {
            string html = string.Format("<script type=\"text/javascript\" language=\"javascript\" src=\"{0}{1}\"></script>", BaseUrl(helper), jsFile.TrimStart('/'));
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString ContentType(this HtmlHelper helper)
        {
            return ContentType(helper, "UTF-8");
        }
        public static MvcHtmlString ContentType(this HtmlHelper helper, string charset)
        {
            return new MvcHtmlString(string.Format("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", charset));
        }
    }
}
