using System;
using System.Collections.Generic;

namespace ShoppingCart.WebApi
{
    internal class InMemoryCartRepository : ICartRepository
    {
        private readonly ICartIdGenerator _cartIdGenerator;
        private Dictionary<Guid, Cart> _carts = new Dictionary<Guid, Cart>();

        public InMemoryCartRepository(ICartIdGenerator cartIdGenerator)
        {
            _cartIdGenerator = cartIdGenerator;
        }

        public Cart GetCart(Guid cartId)
        {
            if (_carts.ContainsKey(cartId))
                return _carts[cartId];

            return null;
        }

        public Guid Add(Cart cart)
        {
            var id = _cartIdGenerator.NewId();
            _carts.Add(id, cart);

            return id;
        }

        public void Save(Cart cart)
        {
            // does nothing
        }
    }

    internal interface ICartIdGenerator
    {
        Guid NewId();
    }

    internal class CartIdGenerator : ICartIdGenerator
    {
        public Guid NewId()
        {
            return Guid.NewGuid();
        }
    }
}