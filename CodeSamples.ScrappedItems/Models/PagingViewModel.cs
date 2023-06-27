using System;

namespace CodeSamples.ScrappedItems.Models{
    [Serializable]
    public class PagingViewModel
    {
        public int Page { get; set; } = 1;
        public int ItemCount { get; set; } = 0;

        private int? pageSize = 50;
        public int? PageSize
        {
            get => ExportAll ? int.MaxValue : pageSize;
            set => pageSize = value;
        }
        public int PageCount => Convert.ToInt32(Math.Ceiling(ItemCount / Convert.ToDouble(PageSize)));
        public int SortOrder { get; set; } = 0;
        public int SortColumn { get; set; } = 0;
		/// <summary>
		/// If <c>ExportAll == true</c>, then this will return <c>null</c>
		/// </summary>
		public int? StartIndex => ExportAll ? default(int?) : ((Page - 1) * PageSize.GetValueOrDefault());
        /// <summary>
        /// If <c>ExportAll == true</c>, then this will return <c>null</c>
        /// </summary>
        public int? EndIndex => ExportAll ? default(int?) : (StartIndex + PageSize.GetValueOrDefault());
        public bool Export { get; set; } = false;
        public bool ExportAll { get; set; } = false;
    }
}