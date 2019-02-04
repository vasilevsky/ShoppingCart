using System;

namespace ShoppingCart.WebApi.Client
{
    public class CartData
    {
        public Guid Id { get; set; }

        public ItemData[] Items { get; set; }
    }
}