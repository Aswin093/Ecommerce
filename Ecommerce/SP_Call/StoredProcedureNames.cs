namespace Ecommerce.Sp_Call
{
    public static class StoredProcedureNames
    {
        public const string CreateOrder = "sp_CreateOrder";
        public const string GetOrdersByCustomer = "sp_GetOrdersByCustomer";
        public const string DeleteOrder = "sp_DeleteOrder";
        public const string AddToCart = "sp_AddToCart";
        public const string GetCartByCustomer = "sp_GetCartByCustomer";
        public const string UpdateCartQuantity = "sp_UpdateCartQuantity";
        public const string RemoveFromCart = "sp_RemoveFromCart";
        public const string GetCartItemsByCustomer = "sp_GetCartItemsByCustomer";
        public const string GetCustomerAddresses = "sp_GetCustomerAddresses";
        public const string GetCustomerAddress = "sp_GetCustomerAddress";
        public const string CreateCustomerAddress = "sp_CreateCustomerAddress";
        public const string UpdateCustomerAddress = "sp_UpdateCustomerAddress";
        public const string DeleteCustomerAddress = "sp_DeleteCustomerAddress";
        public const string CustomerLoginByEmail = "sp_CustomerLoginByEmail";
        public const string CustomerSignup = "sp_CustomerSignup";
        public const string GetCustomerProfile = "sp_GetCustomerProfile";
        public const string UpdateCustomerImage = "sp_UpdateCustomerImage";
        public const string GetAllDealers = "sp_GetAllDealers";
        public const string GetDealerById = "sp_GetDealerById";
        public const string DealerLoginByEmail = "sp_DealerLoginByEmail";
        public const string DealerSignup = "sp_DealerSignup";
        public const string GetAllProducts = "sp_GetAllProducts";
        public const string GetProductById = "sp_GetProductById";
        public const string GetImagesByProductId = "sp_GetImagesByProductId";
        public const string GetProductDetails = "sp_GetProductDetails";
        public const string SearchProducts = "sp_SearchProducts";
        public const string AddProduct = "sp_AddProduct";
        public const string GetProductsByDealer = "sp_GetProductsByDealer";
        public const string UpdateProduct = "sp_UpdateProduct";
        public const string DeleteProduct = "sp_DeleteProduct";
        public const string AddProductImage = "sp_AddProductImage";

    }
}