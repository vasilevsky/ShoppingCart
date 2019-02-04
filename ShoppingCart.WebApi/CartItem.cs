using System;

namespace ShoppingCart.WebApi
{
    public class CartItem
    {
        public CartItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public int ProductId { get; }

        public int Quantity { get; private set; }

        public void SetQuantity(int quantity)
        {
            if (quantity < 1)
                throw new InvalidOperationException("Quantity cannot be negative");

            Quantity = quantity;
        }
    }
}