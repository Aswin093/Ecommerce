using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Data;

namespace Ecommerce.Repository
{
    public class CartProductRepository : ICartProductRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public CartProductRepository(IConfiguration configuration)
        {
           
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public List<CartProduct> GetCartItemsByCustomer(int customerID)
        {
           
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", customerID)
            };

           
            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetCartItemsByCustomer, reader =>
            {
                List<CartProduct> cartItems = new();
                while (reader.Read())
                {
                    cartItems.Add(new CartProduct
                    {
                        CartID = reader.GetInt32("CartID"),
                        ProductID = reader.GetInt32("ProductID"),
                        ProductName = reader.GetString("ProductName"),
                        ImagePath = reader.GetString("ImagePath"),
                        Price = reader.GetDecimal("Price"),
                        Quantity = reader.GetInt32("Quantity")
                    });
                }
                return cartItems;
            }, parameters);
        }
    }
}