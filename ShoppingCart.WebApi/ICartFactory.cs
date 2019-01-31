using System;

namespace ShoppingCart.WebApi
{
    public interface ICartFactory
    {
        Cart GetOrCreate(Guid cartId);
    }
}
