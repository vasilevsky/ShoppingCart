using System;

namespace ShoppingCart.WebApi
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Either<Guid, Failure> AddItem(Guid cartId, AddItemData itemData)
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

        internal Guid CreateCart(AddItemData itemData)
        {
            var cart = new Cart();
            cart.Add(itemData.ProductId, itemData.Quantity);

            return _cartRepository.Add(cart);
        }
    }

    public class NotFound : Failure { }

    public class Either<TUnit, TFailure>
        where TFailure : Failure
    {
        public TUnit Unit { get; }

        public TFailure Failure { get; }

        public Either(TFailure failure)
        {
            Failure = failure;
        }

        public Either(TUnit result)
        {
            Unit = result;
        }

        public static implicit operator Either<TUnit, TFailure>(TUnit result)
        {
            return new Either<TUnit, TFailure>(result);
        }

        public static implicit operator Either<TUnit, TFailure>(TFailure failure)
        {
            return new Either<TUnit, TFailure>(failure);
        }
    }

    public abstract class Failure
    {
    }
}
