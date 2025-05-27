namespace Ecommerce.Model
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int DealerID { get; set; }
        public int Quantity { get; set; }
        public decimal price { get; set; }
        public DateTime OrderDate { get; set; }
        public int AddressId {  get; set; }
        public string? ProductName { get; set; }
        public string? DealerName { get; set; }
        public string? imagepath { get; set; }
        public string? companyname { get; set; }





    }
    public class OrderRequestDto
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int DealerID { get; set; }
        public int Quantity { get; set; }
        public int AddressId { get; set; }
    }
}
