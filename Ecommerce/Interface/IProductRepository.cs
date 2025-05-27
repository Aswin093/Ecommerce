using Ecommerce.Model;
using System.Collections.Generic;

namespace Ecommerce.Interface
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
    }
}