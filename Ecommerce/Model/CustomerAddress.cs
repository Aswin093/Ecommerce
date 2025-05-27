namespace Ecommerce.Model
{
    public class CustomerAddress
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public string?  Address { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
    }
}