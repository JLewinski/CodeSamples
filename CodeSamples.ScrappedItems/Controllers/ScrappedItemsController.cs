using CodeSamples.Helpers.DataAccess;
using CodeSamples.ScrappedItems.Models;
using System;
using System.Web.Mvc;

namespace CodeSamples.ScrappedItems.Controllers
{
    public class ScrappedItemsController : Controller
    {
        public ActionResult AllScrappedPartsReport(ScrappedItemsListViewModel viewModel)
        {

            viewModel.Results = ScrappedItemsReport.GetReport(viewModel);
            ViewBag.Title = "Scrapped Items";

            return View(viewModel);
        }

        public ActionResult ExportAllScrappedPartsReport(ScrappedItemsListViewModel viewModel)
        {
            viewModel.ExportAll = true;
            var data = ScrappedItemsReport.GetReport(viewModel);
            return Json(data);//ExcelResult.Create(data, "All Scrapped Items Report");
        }

        public ActionResult UnscrapeItem(int id, ScrapedAreas area, string reason, string username, string password)
        {
            bool success;
            string errorMessage = null;

            bool approved = true;//GetScrappedAuthorizationByUserAction.GetScrappedAuthorizationByUser(UserPermissionLevel.Supervisor, username, password, out int userId);
            if (!approved)
            {
                errorMessage = "You do not have permission to unscrap items.";
                success = false;
                return Json(new { success, errorMessage });
            }

            if(string.IsNullOrWhiteSpace(reason))
            {
                errorMessage = "A reason must be provided.";
                success = false;
                return Json(new { success, errorMessage });
            }

            try
            {
                //TODO: Update all stored procedures to use the proper parameters
                switch (area)
                {
                    /*case ScrapedAreas.DryBatch:
                        success = DryBatchScrappedItemsData.UnScrap(id, reason, userId);
                        break;
                    case ScrapedAreas.FiredLog:
                        success = FiredLogScrappedItemsData.UnScrap(id, reason, userId);
                        break;
                    case ScrapedAreas.GreenLog:
                        success = GreenLogScrappedItemsData.UnScrap(id, reason, userId);
                        break;
                    case ScrapedAreas.KibbleMix:
                        success = KibbleMixScrappedItemsData.UnScrap(id, reason, userId);
                        break;
                    case ScrapedAreas.CutParts:
                        success = CutPartsScrappedItemsData.UnScrap(id, reason, userId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(area), area, null);*/
                    default:
                        success = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                success = false;
                ElmahErrorLogging.Log(ex);
                if (errorMessage == null)
                {
                    errorMessage = "There was an error unscraping the item.";
                }
            }

            return Json(new { success, errorMessage });
        }
    }
}
