using Ecommerce.Model;
using System.Collections.Generic;

namespace Ecommerce.Interface
{
    public interface IDealerProductRepository
    {
        void AddProduct(Product product);
        List<Product> GetDealerProducts(int dealerId);
        Product GetProduct(int id);
        bool UpdateProduct(int id, Product product);
        bool DeleteProduct(int id);
    }
}