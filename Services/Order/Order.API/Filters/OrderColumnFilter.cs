namespace Order.API.Filters
{
    public class OrderColumnFilter
    {
        public OrderColumnFilter()
        {
            
        }

        public int OwnerId { get; set; }
        public bool? IsPaid { get; set; }
    }
}