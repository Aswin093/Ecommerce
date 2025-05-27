using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Data;

namespace Ecommerce.Repository
{
    public class CustomerLoginRepository : ICustomerLoginRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public CustomerLoginRepository(IConfiguration configuration)
        {
            // Retrieve the connection string from configuration for SQL Server database access
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public CustomerLoginDetails GetCustomerByEmail(string email)
        {
            // Define parameters for the stored procedure
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@Email", email)
            };

            // Execute the stored procedure and process results
            return _sqlExecutor.ExecuteReader(StoredProcedureNames.CustomerLoginByEmail, reader =>
            {
                if (reader.Read())
                {
                    return new CustomerLoginDetails
                    {
                        CustomerID = reader.GetInt32("CustomerID"),
                        CustomerName = reader.GetString("CustomerName"),
                        PasswordHash = reader.GetString("PasswordHash")
                    };
                }
                return null;
            }, parameters);
        }
    }
}