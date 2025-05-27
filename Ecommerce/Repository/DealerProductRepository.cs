using Ecommerce.Model;
using Ecommerce.Interface;
using Ecommerce.ExecuteQuery;
using Ecommerce.Sp_Call;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Data;

namespace Ecommerce.Repository
{
    public class DealerProductRepository : IDealerProductRepository
    {
        private readonly SqlExecutor _sqlExecutor;

        public DealerProductRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _sqlExecutor = new SqlExecutor(connectionString);
        }

        public void AddProduct(Product product)
        {
            // Validate DealerID
            var dealerCount = (int)_sqlExecutor.ExecuteScalar(
                "SELECT COUNT(*) FROM Dealers WHERE DealerID = @DealerID",
                new SqlParameter("@DealerID", product.DealerID)
            );

            if (dealerCount == 0)
            {
                throw new Exception($"DealerID {product.DealerID} does not exist.");
            }

            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@DealerID", product.DealerID),
                new SqlParameter("@ProductName", product.ProductName ?? (object)DBNull.Value),
                new SqlParameter("@Description", product.Description ?? (object)DBNull.Value),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@Stock", product.Stock),
                new SqlParameter("@ImagePath", product.ImagePath ?? (object)DBNull.Value),
                new SqlParameter("@CreatedDate", DateTime.Now),
                new SqlParameter("@UpdatedDate", DateTime.Now)
            };

            _sqlExecutor.ExecuteNonQuery(StoredProcedureNames.AddProduct, parameters);
        }

        public List<Product> GetDealerProducts(int dealerId)
        {
            SqlParameter[] parameters = new[]
            {
        new SqlParameter("@DealerID", dealerId)
    };

            return _sqlExecutor.ExecuteReader(StoredProcedureNames.GetProductsByDealer, reader =>
            {
                List<Product> products = new();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = reader.GetInt32("ProductID"),
                        DealerID = reader.GetInt32("DealerID"),
                        ProductName = reader.GetString("ProductName"),
                        Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                        Price = reader.GetDecimal("Price"),
                        Stock = reader.GetInt32("Stock"),
                        ImagePath = reader.IsDBNull("ImagePath") ? null : reader.GetString("ImagePath"),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                        UpdatedDate = reader.GetDateTime("UpdatedDate")
                    });
                }
                return products;
            }, parameters);
        }
        public Product GetProduct(int id)
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
                        Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                        Price = reader.GetDecimal("Price"),
                        Stock = reader.GetInt32("Stock"),
                        ImagePath = reader.IsDBNull("ImagePath") ? null : reader.GetString("ImagePath"),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                    };
                }
                return null;
            }, parameters);
        }

        public bool UpdateProduct(int id, Product product)
        {
            SqlParameter[] parameters = new[]
            {
        new SqlParameter("@ProductID", id),
        new SqlParameter("@ProductName", product.ProductName ?? (object)DBNull.Value),
        new SqlParameter("@Description", product.Description ?? (object)DBNull.Value),
        new SqlParameter("@Price", product.Price),
        new SqlParameter("@Stock", product.Stock),
        new SqlParameter("@ImagePath", product.ImagePath ?? (object)DBNull.Value),
        new SqlParameter("@UpdatedDate", DateTime.Now)
    };

            return _sqlExecutor.ExecuteRowsAffected(StoredProcedureNames.UpdateProduct, parameters);
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                _sqlExecutor.ExecuteTransaction((conn, transaction) =>
                {
                    // Check if product exists
                    using SqlCommand checkCmd = new("SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID", conn, transaction);
                    checkCmd.Parameters.AddWithValue("@ProductID", id);
                    int productCount = (int)checkCmd.ExecuteScalar();

                    if (productCount == 0)
                    {
                        throw new Exception("Product not found.");
                    }

                    // Delete related AddToCart records
                    using SqlCommand cartCmd = new("DELETE FROM AddToCart WHERE ProductID = @ProductID", conn, transaction);
                    cartCmd.Parameters.AddWithValue("@ProductID", id);
                    cartCmd.ExecuteNonQuery();

                    // Delete the product
                    SqlParameter[] parameters = new[]
                    {
                        new SqlParameter("@ProductID", id)
                    };

                    using SqlCommand productCmd = new(StoredProcedureNames.DeleteProduct, conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    productCmd.Parameters.AddRange(parameters);
                    productCmd.ExecuteNonQuery();
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}