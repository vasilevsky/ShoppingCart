using System;

namespace ShoppingCart.WebApi
{
    public interface ICartRepository
    {
        Cart GetCart(Guid cartId);

        Guid Add(Cart cart);

        void Save(Cart cart);
    }
}