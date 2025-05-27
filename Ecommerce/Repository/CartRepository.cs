using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Data;

namespace Ecommerce.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly SqlExecutor? _sqlExecutor;

        public CartRepository(IConfiguration configuration)
        {

            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            
            _sqlExecutor = new SqlExecutor(connectionString);
        }
        //insert
        public void AddToCart(AddToCart cartItem)
        {
           
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", cartItem.CustomerID),
                new SqlParameter("@ProductID", cartItem.ProductID),
                new SqlParameter("@Quantity", cartItem.Quantity)
            };

           
            _sqlExecutor.ExecuteNonQuery(StoredProcedureNames.AddToCart, parameters);
        }
        //select by customer id
        public List<CartItemResponse> GetCartByCustomer(int customerID)
        {
            
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", customerID)
            };

          
            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetCartByCustomer, reader =>
            {
                List<CartItemResponse> cartItems = new();
                while (reader.Read())
                {
                    cartItems.Add(new CartItemResponse
                    {
                        CartID = reader.GetInt32("CartID"),
                        ProductID = reader.GetInt32("ProductID"),
                        Quantity = reader.GetInt32("Quantity"),
                        ProductName = reader.GetString("ProductName"),
                        Price = reader.GetDecimal("Price"),
                        ImagePath = reader.IsDBNull("ImagePath") ? null : reader.GetString("ImagePath")
                    });
                }
                return cartItems;
            }, parameters);
        }

        public bool UpdateCartQuantity(UpdateCartRequest request)
        {
           
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CartID", request.CartID),
                new SqlParameter("@ProductID", request.ProductID),
                new SqlParameter("@Quantity", request.Quantity)
            };
            return _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.UpdateCartQuantity, parameters);
        }

        public bool RemoveFromCart(int cartID)
        {
           
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CartID", cartID)
            };

            return _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.RemoveFromCart, parameters);
        }
    }
}