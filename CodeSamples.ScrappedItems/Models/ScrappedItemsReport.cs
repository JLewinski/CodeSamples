using CodeSamples.Helpers.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using CodeSamples.Extensions.DataAccess;

namespace CodeSamples.ScrappedItems.Models
{
    public class ScrappedItemsReport
    {
        public ScrappedItemsReport(SqlDataReader reader)
        {
            ScrappedDateTime = reader.GetValue<DateTime>(nameof(ScrappedDateTime));
            AreaScrapped = reader.GetValue<ScrapedAreas>(nameof(AreaScrapped));
            IdScrapped = reader.GetValue<int>(nameof(IdScrapped));
            DisplayId = reader.GetValue<string>(nameof(DisplayId));
            QuantityScrapped = reader.GetValue<double>(nameof(QuantityScrapped));
            ScrappedUserFullName = reader.GetValue<string>(nameof(ScrappedUserFullName));
            ScrappedReason = reader.GetValue<string>(nameof(ScrappedReason));
            ReversedDateTime = reader.GetValue<DateTime?>(nameof(ReversedDateTime));
            ReversedUserFullName = reader.GetValue<string>(nameof(ReversedUserFullName));
            ReversedReason = reader.GetValue<string>(nameof(ReversedReason));
        }

        [DisplayName("Scrapped Date Time")]
        public DateTime ScrappedDateTime { get; set; }
        [DisplayName("Area Scrapped")]
        public ScrapedAreas AreaScrapped { get; set; }
        //[ExcludeExcelResult]
        public int IdScrapped { get; set; }
        [DisplayName("Batch ID")]
        public string DisplayId { get; set; }
        [DisplayName("Quantity Scrapped")]
        public double QuantityScrapped { get; set; }
        [DisplayName("Scrapped By")]
        public string ScrappedUserFullName { get; set; }
        [DisplayName("Scrapped Reason")]
        public string ScrappedReason { get; set; }
        [DisplayName("Reversed Date Time")]
        public DateTime? ReversedDateTime { get; set; }
        [DisplayName("Reversed By")]
        public string ReversedUserFullName { get; set; }
        [DisplayName("Reversed Reason")]
        public string ReversedReason { get; set; }

        public static List<ScrappedItemsReport> GetReport(ScrappedItemsListViewModel viewModel)
        {
            //viewModel.ValidateAndFixSearchDates();
            viewModel.Results = StoredProcedureHelper.GetList("Reports_Scrapped", viewModel.GetParameters(), r => new ScrappedItemsReport(r), out int itemCount);
            viewModel.ItemCount = itemCount;
            return viewModel.Results;
        }
    }
}