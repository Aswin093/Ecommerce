using Ecommerce.Model;
using System.Collections.Generic;

namespace Ecommerce.Interface
{
    public interface ICartRepository
    {
        //insert
        void AddToCart(AddToCart cartItem);
        List<CartItemResponse> GetCartByCustomer(int customerID);
        bool UpdateCartQuantity(UpdateCartRequest request);
        bool RemoveFromCart(int cartID);
    }
}