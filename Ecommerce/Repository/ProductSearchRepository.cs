using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace Ecommerce.Repository
{
    public class ProductSearchRepository : IProductSearchRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public ProductSearchRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public async Task<List<ProductSearchDto>> SearchProducts(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Search term cannot be empty.");
            }

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@SearchTerm", name)
            };

            return await Task.Run(() => _sqlExecutor.ExecuteReader(StoredProcedureNames.SearchProducts, reader =>
            {
                List<ProductSearchDto> products = new();
                while (reader.Read())
                {
                    products.Add(new ProductSearchDto
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        ProductName = reader.GetString("ProductName"),
                        Image = reader["ImagePath"] != DBNull.Value ? reader.GetString("ImagePath") : null
                    });
                }
                return products;
            }, parameters));
        }
    }
}