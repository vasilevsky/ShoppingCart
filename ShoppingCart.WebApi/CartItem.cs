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

        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}