namespace Basket.API.Dto
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(int pageNumber, int pageSize, int totalPages, int totalRecords, T data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
            this.data = data;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public T data { get; set; }
    }
}