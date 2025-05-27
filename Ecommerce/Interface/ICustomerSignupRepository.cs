using Ecommerce.Model;

namespace Ecommerce.Interface
{
    public interface ICustomerSignupRepository
    {
        bool SignupCustomer(CustomerSignupModel model);
    }
}