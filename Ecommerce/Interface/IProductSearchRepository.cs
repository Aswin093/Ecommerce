using Ecommerce.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Interface
{
    public interface IProductSearchRepository
    {
        Task<List<ProductSearchDto>> SearchProducts(string name);
    }
}