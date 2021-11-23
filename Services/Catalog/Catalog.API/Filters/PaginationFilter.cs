namespace Catalog.API.Filters
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        public PaginationFilter()
        {
        }
        
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber > 0 ? pageNumber : 1;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}