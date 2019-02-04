using System;

namespace ShoppingCart.WebApi
{
    /// <summary>
    /// Implemmnts application logic over cart and persists the state.
    /// </summary>
    public class CartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Either<Guid, Failure> AddItem(Guid cartId, ItemData itemData)
        {
            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
            {
                return new NotFound();
            }

            cart.Add(itemData.ProductId);
            _cartRepository.Save(cart);

            return cartId;
        }

        public Either<Guid, Failure> CreateCart(ItemData itemData)
        {
            var cart = new Cart();
            cart.Add(itemData.ProductId, itemData.Quantity);

            return _cartRepository.Add(cart);
        }

        public Either<Guid, Failure> ClearCart(Guid cartId)
        {
            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
                return new NotFound();

            cart.ClearAllItems();

            return cartId;
        }

        public Either<Guid, Failure> DeleteCartItem(Guid cartId, int productId)
        {
            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
                return new NotFound();

            cart.DeleteItemBy(productId);

            return cartId;
        }

        public Either<Guid, Failure> UpdateItem(Guid cartId, int productId, int quantity)
        {
            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
                return new NotFound();

            cart.UpdateItem(productId, quantity);

            return cartId;
        }
    }
}
