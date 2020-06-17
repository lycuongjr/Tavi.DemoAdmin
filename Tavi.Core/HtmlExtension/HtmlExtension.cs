using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tavi.Core.Models;

namespace Tavi.Core.HtmlEx
{
    public static class HtmlExtension
    {
        #region Button
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText)
        {
            return Button(htmlHelper, buttonText, "btn btn-default");
        }
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText, string className)
        {
            return new MvcHtmlString(string.Format("<input type=\"button\" value=\"{0}\" class=\"{1}\"/>", buttonText, className));
        }
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText, string className, string buttonId)
        {
            return new MvcHtmlString(string.Format("<input type=\"button\" value=\"{0}\" id=\"{1}\" class=\"{2}\"/>", buttonText, buttonId, className));
        }
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string buttonText, string className, string buttonId, string icon)
        {
            return new MvcHtmlString(string.Format("<div  id=\"{1}\" class=\"{2}\"><i class=\"{3}\"></i> {0}</div>", buttonText, buttonId, className, icon));
        }

        #endregion button

        #region CheckBox
        public static MvcHtmlString CheckBoxEx(this HtmlHelper htmlHelper, string name, string lable, bool isChecked = false, bool isDisabled = false, object htmlAttributes = null)
        {
            var attributes = new Dictionary<string, object>();
            var id = name;
            attributes.Add("class", "custom-control-input");
            if (isDisabled)
            {
                attributes.Add("disabled", "disabled");
            }
            if (htmlAttributes != null)
            {
                foreach (PropertyInfo property in htmlAttributes.GetType().GetProperties())
                {
                    object propertyValue = property.GetValue(htmlAttributes, null);
                    attributes.Add(property.Name, propertyValue);
                    if(property.Name == "id")
                    {
                        id = propertyValue.ToString();
                    }
                }
            }
            string html = "<div class='custom-control custom-checkbox'>" +
                            htmlHelper.CheckBox(name, isChecked, attributes) +
                          "<label for='" + id + "' class='custom-control-label font-weight-normal'>" + lable + "</label>" +
                        "</div>";
            return new MvcHtmlString(html);
        }
        #endregion

        #region Radio button
        public static MvcHtmlString RadioButonEx(this HtmlHelper htmlHelper, string name, object value, string lable, bool isChecked = false, bool isDisabled = false, object htmlAttributes = null)
        {
            var attributes = new Dictionary<string, object>();
            var id = name;
            attributes.Add("class", "custom-control-input");
            if (isDisabled)
            {
                attributes.Add("disabled", "disabled");
            }
            if (htmlAttributes != null)
            {
                foreach (PropertyInfo property in htmlAttributes.GetType().GetProperties())
                {
                    object propertyValue = property.GetValue(htmlAttributes, null);
                    attributes.Add(property.Name, propertyValue);
                    if (property.Name == "id")
                    {
                        id = propertyValue.ToString();
                    }
                }
            }
            string html = "<div class='custom-control custom-radio'>" +
                            htmlHelper.RadioButton(name, value, isChecked, attributes) +
                          "<label for='" + id + "' class='custom-control-label font-weight-normal'>" + lable + "</label>" +
                        "</div>";
            return new MvcHtmlString(html);
        }
        #endregion

        #region dropdown list
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItemExtend> selectList, string optionLabel)
        {
            var html = String.Empty;
            html += "<select name=\"" + name + "\" id=\"" + name + "\">";

            if (!string.IsNullOrEmpty(optionLabel))
            {
                html += "<option value=\"\">" + optionLabel + "</option>";
            }

            if (selectList != null && selectList.Any())
            {
                foreach(var item in selectList)
                {
                    html += "<option value=\"" + (item.Value ?? "") + "\" data-description=\"" + (item.Description ?? "") + "\"" + (item.Selected ? " selected" : "") + "\">" + item.Text + "</option>";
                }
            }

            html += "</select>";

            return new MvcHtmlString(html);
        }
        #endregion

        #region Date Picker
        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, DateTime? value = null, object htmlAttributes = null)
        {
            string val = value.HasValue ? value.Value.ToString("dd/MM/yyyy"): null;
            return DatePicker(htmlHelper,name,val,htmlAttributes);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, string value, object htmlAttributes = null)
        {
            string html = htmlHelper.TextBox(name, value, htmlAttributes).ToString();
            html += "<script>$('#" + name + "').datepicker({language: 'vi',autoclose: true, format: 'dd/mm/yyyy',})</script>";
            return new MvcHtmlString(html);
        }
        #endregion

        #region Status
        public static MvcHtmlString StatusDisplay(this HtmlHelper htmlHelper, bool? isActive)
        {
            string html = "<i class='fa " + (isActive == true ? "fa-check-circle text-success' title='Còn sử dụng'" : "fa-ban text-danger' title='Không sử dụng'") + "></i>";
            return new MvcHtmlString(html);
        }

        #endregion

        #region File
        public static string GetFileSize(this HtmlHelper htmlHelper, long fileSize)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (fileSize >= 1024 && order < sizes.Length - 1)
            {
                order++;
                fileSize = fileSize / 1024;
            }
            string result = String.Format("{0:0.##} {1}", fileSize, sizes[order]);
            return result;
        }
        public static string GetFileName(this HtmlHelper htmlHelper, string fullName)
        {
            if (fullName.Contains('_'))
            {
                int index = fullName.IndexOf('_');
                fullName = fullName.Substring(index + 1);
            }
            return fullName;
        }
        #endregion
    }
}
