using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.ExecuteQuery;
using Ecommerce.Sp_Call;
using Microsoft.Data.SqlClient;

namespace Ecommerce.Repository
{
    public class DealerSignupRepository : IDealerSignupRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public DealerSignupRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public void DealerSignup(DealerSignupModel model)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@DealerName", model.DealerName),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@PasswordHash", Helper.PasswordHasher.HashPassword(model.Password)),
                new SqlParameter("@ContactNumber", (object?)model.ContactNumber ?? DBNull.Value),
                new SqlParameter("@CompanyName", (object?)model.CompanyName ?? DBNull.Value)
            };

            _sqlExecutor.ExecuteNonQuery(StoredProcedureNames.DealerSignup, parameters);
        }
    }
}