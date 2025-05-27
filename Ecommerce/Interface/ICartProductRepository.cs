using Ecommerce.Model;

namespace Ecommerce.Interface
{
    public interface ICartProductRepository
    {
        List<CartProduct> GetCartItemsByCustomer(int customerID);
    }
}
