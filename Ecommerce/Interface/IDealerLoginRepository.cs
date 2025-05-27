using Ecommerce.Model;

namespace Ecommerce.Interface
{
    public interface IDealerLoginRepository
    {
        DealerLoginDetails GetDealerByEmail(string email);
    }
}
