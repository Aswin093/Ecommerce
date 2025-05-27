using Ecommerce.Model;

namespace Ecommerce.Interface
{
    public interface ICustomerLoginRepository
    {
        CustomerLoginDetails GetCustomerByEmail(string email);
    }
}
