using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Data;

namespace Ecommerce.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public CustomerRepository(IConfiguration configuration)
        {
            // Retrieve the connection string from configuration for SQL Server database access
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public CustomerProfileResponse GetCustomerProfile(int customerId)
        {
            // Define parameters for the stored procedure
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", customerId)
            };

            // Execute the stored procedure and process results
            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetCustomerProfile, reader =>
            {
                if (reader.Read())
                {
                    return new CustomerProfileResponse
                    {
                        CustomerId = reader.GetInt32("CustomerId"),
                        CustomerName = reader.GetString("CustomerName"),
                        CustomerEmail = reader.GetString("Email"),
                        ContactNumber = reader.GetString("ContactNumber"),
                        Image = reader.IsDBNull("Image") ? "/images/default-profile.jpg" : reader.GetString("Image")
                    };
                }
                return null;
            }, parameters);
        }

        public bool UpdateCustomerImage(int customerId, string imageUrl)
        {
            // Define parameters for the stored procedure
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@ProfileImage", imageUrl)
            };

            // Execute the stored procedure and check rows affected
            return _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.UpdateCustomerImage, parameters);
        }
    }
}