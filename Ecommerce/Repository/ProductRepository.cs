using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Collections.Generic;
using System.Data;

namespace Ecommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public ProductRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public List<Product> GetAllProducts()
        {
            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetAllProducts, reader =>
            {
                List<Product> products = new();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        DealerID = reader.GetInt32("DealerID"),
                        ProductName = reader.GetString("ProductName"),
                        Description = reader.GetString("Description"),
                        Price = reader.GetDecimal("Price"),
                        Stock = reader.GetInt32("Stock"),
                        ImagePath = reader.GetString("ImagePath"),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                        UpdatedDate = reader.GetDateTime("UpdatedDate")
                    });
                }
                return products;
            });
        }

        public Product GetProductById(int id)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@ProductID", id)
            };

            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetProductById, reader =>
            {
                if (reader.Read())
                {
                    return new Product
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        DealerID = reader.GetInt32("DealerID"),
                        ProductName = reader.GetString("ProductName"),
                        Description = reader.GetString("Description"),
                        Price = reader.GetDecimal("Price"),
                        Stock = reader.GetInt32("Stock"),
                        ImagePath = reader.GetString("ImagePath"),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                        UpdatedDate = reader.GetDateTime("UpdatedDate")
                    };
                }
                return null;
            }, parameters);
        }
    }
}