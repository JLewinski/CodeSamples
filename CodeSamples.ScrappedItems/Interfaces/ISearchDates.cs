using System;

namespace CodeSamples.ScrappedItems.Interfaces {
    public interface ISearchDates
	{
		DateTime? StartDate { get; set; }
		DateTime? EndDate { get; set; }
	}
}