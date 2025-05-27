using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using Ecommerce.Helper;

namespace Ecommerce.Repository
{
    public class CustomerSignupRepository : ICustomerSignupRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public CustomerSignupRepository(IConfiguration configuration)
        {
     
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public bool SignupCustomer(CustomerSignupModel model)
        {
           
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@CustomerName", model.CustomerName),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@PasswordHash", PasswordHasher.HashPassword(model.Password)),
                new SqlParameter("@ContactNumber", model.ContactNumber)
            };

            try
            {
                
                _sqlExecutor.ExecuteNonQuery(StoredProcedureNames.CustomerSignup, parameters);
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
}