using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Data;

namespace Ecommerce.Repository
{
    public class DealerRepository : IDealerRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public DealerRepository(IConfiguration configuration)
        {
            // Retrieve the connection string from configuration for SQL Server database access
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public List<Dealer> GetAllDealers()
        {
            // Execute the stored procedure and process results
            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetAllDealers, reader =>
            {
                List<Dealer> dealers = new();
                while (reader.Read())
                {
                    dealers.Add(new Dealer
                    {
                        DealerID = reader.GetInt32("DealerID"),
                        DealerName = reader.GetString("DealerName"),
                        Email = reader.GetString("Email"),
                        CompanyName = reader.GetString("CompanyName"),
                        ContactNumber = reader.GetString("ContactNumber")
                    });
                }
                return dealers;
            }, null);
        }

        public Dealer GetDealerById(int dealerId)
        {
            // Define parameters for the stored procedure
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@DealerID", dealerId)
            };

            // Execute the stored procedure and process results
            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetDealerById, reader =>
            {
                if (reader.Read())
                {
                    return new Dealer
                    {
                        DealerID = reader.GetInt32("DealerID"),
                        DealerName = reader.GetString("DealerName"),
                        Email = reader.GetString("Email"),
                        CompanyName = reader.GetString("CompanyName"),
                        ContactNumber = reader.GetString("ContactNumber")
                    };
                }
                return null;
            }, parameters);
        }
    }
}