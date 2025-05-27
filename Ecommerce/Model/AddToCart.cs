namespace Ecommerce.Model
{
    public class AddToCart
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsCheckedOut { get; set; }

    }
    public class UpdateCartRequest
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemResponse
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
    }
}
