namespace CustomerRelationshipManagementAPI.Core.Helpers
{
    public class PaginationMetaData
    {
        public PaginationMetaData(int pageNumber, int pageSize, int totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            PagesCount = (int) Math.Ceiling(totalCount / (double)pageSize);
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PagesCount { get; set; }

    }
}
