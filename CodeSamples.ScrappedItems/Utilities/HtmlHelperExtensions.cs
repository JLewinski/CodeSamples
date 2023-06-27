using CodeSamples.ScrappedItems.Models;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CodeSamples.ScrappedItems.Utilities
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString PagerSection(this HtmlHelper helper, string id, string form, PagingViewModel viewModel)
        {
            return PagerSection(helper, id, form, viewModel.Page, viewModel.PageCount, viewModel.PageSize, viewModel.ItemCount);
        }

        public static MvcHtmlString PagerSection(this HtmlHelper helper, string id, string form, int page, int pageCount, int? pageSize, int itemCount)
        {
            var viewData = new ViewDataDictionary
            {
                ["id"] = id,
                ["form"] = form,
                ["page"] = page,
                ["pageCount"] = pageCount,
                ["pageSize"] = pageSize ?? 50,
                ["itemCount"] = itemCount
            };
            return helper.Partial("PagerSection", viewData);
        }

        public static MvcHtmlString PagerHiddenInputs(this HtmlHelper helper, PagingViewModel viewModel)
        {
            var viewData = new ViewDataDictionary();
            return helper.Partial(nameof(PagerHiddenInputs), viewModel);
        }
    }
}