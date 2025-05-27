namespace Ecommerce.Model
{
    public class CustomerLoginDetails
    {
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? PasswordHash { get; set; }
    }
}