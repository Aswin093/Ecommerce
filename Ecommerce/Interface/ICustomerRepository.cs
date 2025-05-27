using Ecommerce.Model;

namespace Ecommerce.Interface
{
    public interface ICustomerRepository
    {
        CustomerProfileResponse GetCustomerProfile(int customerId);
        bool UpdateCustomerImage(int customerId, string imageUrl);
    }
}
