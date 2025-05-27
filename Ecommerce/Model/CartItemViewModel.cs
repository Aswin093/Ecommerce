namespace Ecommerce.Model
{
    public class CartItemViewModel
    {
        public int CartID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
