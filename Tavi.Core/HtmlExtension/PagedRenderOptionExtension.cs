using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList.Mvc;
namespace Tavi.Core.HtmlEx
{
    public class PagedRenderOptionExtension : PagedList.Mvc.PagedListRenderOptions
    {

        public static PagedListRenderOptions ClassicCustomPage
        {

            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToLastPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayItemSliceAndTotal = true,
                    EllipsesFormat = "&#8230;",
                    LinkToFirstPageFormat = "««",
                    LinkToPreviousPageFormat = "«",
                    LinkToIndividualPageFormat = "{0}",
                    LinkToNextPageFormat = "»",
                    LinkToLastPageFormat = "»»",
                    PageCountAndCurrentLocationFormat = "Trang {0} / {1}.",
                    ItemSliceAndTotalFormat = "Hiển thị từ  {0} - {1} trên tổng số {2} bản ghi.",
                    FunctionToDisplayEachPageNumber = null,
                    ClassToApplyToFirstListItemInPager = null,
                    ClassToApplyToLastListItemInPager = null,
                    ContainerDivClasses = new[] { "pagination-container" },
                    UlElementClasses = new[] { "pagination" },
                    LiElementClasses = Enumerable.Empty<string>()
                };
            }
        }
    }
}
