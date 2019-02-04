using System.ComponentModel;

namespace ShoppingCart.WebApi
{
    [Description("Cart item data.")]
    public class ItemData
    {
        [Description("Id of cart item product.")]
        public int ProductId { get; set; }

        [Description("Quantity of the product.")]
        public int Quantity { get; set; }
    }
}
