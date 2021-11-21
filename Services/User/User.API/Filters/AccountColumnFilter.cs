namespace User.API.Filters
{
    public class AccountColumnFilter
    {
        public AccountColumnFilter()
        {
            
        }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
    }
}