using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;

using System.Data;

namespace Ecommerce.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public OrderRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public int CreateOrder(OrderRequestDto orderRequest)
        {
            if (orderRequest.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            if (orderRequest.AddressId <= 0)
                throw new ArgumentException("Invalid AddressId.");

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", orderRequest.CustomerID),
                new SqlParameter("@ProductID", orderRequest.ProductID),
                new SqlParameter("@DealerID", orderRequest.DealerID),
                new SqlParameter("@Quantity", orderRequest.Quantity),
                new SqlParameter("@AddressId", orderRequest.AddressId),
                new SqlParameter
                {
                    ParameterName = "@OrderID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                }
            };

            _sqlExecutor.ExecuteNonQuery(StoredProcedureNames.CreateOrder, parameters);

            return (int)parameters[5].Value;
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", customerId)
            };

            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetOrdersByCustomer, reader =>
            {
                List<Order> orders = new();
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderID = reader.GetInt32("OrderID"),
                        CustomerID = reader.GetInt32("CustomerID"),
                        ProductID = reader.GetInt32("ProductID"),
                        DealerID = reader.GetInt32("DealerID"),
                        Quantity = reader.GetInt32("Quantity"),
                        OrderDate = reader.GetDateTime("OrderDate"),
                        AddressId = reader.GetInt32("AddressId"),
                        ProductName = reader.GetString("ProductName"),
                        imagepath = reader.GetString("imagepath"),
                        price = reader.GetDecimal("price"),
                        DealerName = reader.GetString("DealerName"),
                        companyname = reader.GetString("companyname")
                    });
                }
                return orders;
            }, parameters);
        }

        public int DeleteOrder(int orderId)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@OrderID", orderId),
                new SqlParameter
                {
                    ParameterName = "@DeletedOrderID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                }
            };

            _sqlExecutor.ExecuteNonQuery(StoredProcedureNames.DeleteOrder, parameters);

            if (parameters[1].Value == DBNull.Value)
            {
                throw new Exception("Order not found or already deleted.");
            }

            return (int)parameters[1].Value;
        }
    }
}