using System.Web.Mvc;

namespace CodeSamples.ScrappedItems.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("AllScrappedPartsReport", "ScrappedItems");
        }
    }
}