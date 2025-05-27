namespace Ecommerce.Model
{
    public class Customer
    {
       public int CustomerId {  get; set; }
        public string? CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        = string.Empty;
        public string? contactNumber { get; set; }
        public string? password { get; set; }
        public string? image { get; set; }
     
    }
}
