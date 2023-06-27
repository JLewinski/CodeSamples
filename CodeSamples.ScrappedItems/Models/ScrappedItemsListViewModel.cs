using System;
using System.Collections.Generic;
using System.ComponentModel;
using CodeSamples.Interfaces;
using CodeSamples.ScrappedItems.Interfaces;
using static CodeSamples.ScrappedItems.Utilities.EnumUtilities;

namespace CodeSamples.ScrappedItems.Models{
    public enum ScrapedAreas
    {
        [Description("Kibble Mix")]
        KibbleMix = 3,
        [Description("Dry Batch")]
        DryBatch = 0,
        [Description("Green Log")]
        GreenLog = 2,
        [Description("Fired Log")]
        FiredLog = 1,
        [Description("Cut Parts")]
        CutParts = 4
    }

    public class ScrappedItemsListViewModel : PagingViewModel, ISearchDates, IParameterObject
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ScrapedAreas? Area { get; set; }

        public IEnumerable<ScrapedAreas> Areas => GetEnumValues<ScrapedAreas>();

        /// <summary>
        /// Check this for the following: mwdId, calcinerId, productionId, boxId, etc.
        /// </summary>
        public string SearchText { get; set; }
        public bool? IsScrapped { get; set; }
        
        public List<ScrappedItemsReport> Results { get; set; }

        public object GetParameters()
        {
            return new
            {
                StartDate,
                EndDate,
                Area,
                SearchText,
                IsScrapped,
                StartIndex,
                PageSize,
                SortOrder,
                SortColumn
            };
        }
    }
}