namespace Ecommerce.Model
{
    public class CustomerProfileResponse
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? ContactNumber { get; set; }
        public string? Image { get; set; }
    }
}
