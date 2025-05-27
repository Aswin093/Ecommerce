using Ecommerce.Model;
using System.Collections.Generic;

namespace Ecommerce.Interface
{
    public interface IDealerRepository
    {
        List<Dealer> GetAllDealers();
        Dealer GetDealerById(int dealerId);
    }
}