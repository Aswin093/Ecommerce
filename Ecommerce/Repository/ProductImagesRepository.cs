using Microsoft.Data.SqlClient;
using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.Sp_Call;
using Ecommerce.ExecuteQuery;
using System.Data;

namespace Ecommerce.Repository
{
    public class ProductImagesRepository : IProductImagesRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public ProductImagesRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public async Task<List<ProductImages>> GetImagesByProductId(int productId)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@ProductID", productId)
            };

            return await Task.Run(() => _sqlExecutor.ExecuteReader(StoredProcedureNames.GetImagesByProductId, reader =>
            {
                List<ProductImages> images = new();
                while (reader.Read())
                {
                    images.Add(new ProductImages
                    {
                        ImageId = reader.GetInt32("ImageID"),
                        ProductId = reader.GetInt32("ProductId"),
                        ImagePath = reader["ImagePath"]?.ToString()
                    });
                }
                return images;
            }, parameters));
        }

        public async Task<Product> GetProductDetails(int productId)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@ProductID", productId)
            };

            return await Task.Run(() => _sqlExecutor.ExecuteReader(StoredProcedureNames.GetProductDetails, reader =>
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
            }, parameters));
        }
        public async Task<bool> AddImagePath(int productId, string imagePath)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@ProductID", productId),
                new SqlParameter("@ImagePath", imagePath)
            };

            return await Task.Run(() => _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.AddProductImage, parameters));
        }
    }
}