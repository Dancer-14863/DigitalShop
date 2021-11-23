namespace Catalog.API.Filters
{
    public class ProductColumnFilter
    {
        public ProductColumnFilter()
        {
            
        }

        public string? ProductName { get; set; }
        public int OwnerId { get; set; }
    }
}