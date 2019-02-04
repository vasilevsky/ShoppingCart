using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.WebApi
{
    public class Cart
    {
        private readonly List<CartItem> items = new List<CartItem>();

        public Guid Id { get; }

        public IReadOnlyCollection<CartItem> Items => items;

        public void Add(int productId, int quantity = 1)
        {
            var existingItem = items.FirstOrDefault(m => m.ProductId == productId);
            if (existingItem == null)
            {
                AddNewItem(productId, quantity);
            }
            else
            {
                existingItem.SetQuantity(quantity);
            }
        }

        private void AddNewItem(int productId, int quantity)
        {
            var newItem = new CartItem(productId, quantity);
            items.Add(newItem);
        }

        public void DeleteItemBy(int productId)
        {
            var item = items.FirstOrDefault(m => m.ProductId == productId);
            if (item != null)
                items.Remove(item);
        }

        public void ClearAllItems()
        {
            items.Clear();
        }

        public void UpdateItem(int productId, int quantity)
        {
            var item = items.FirstOrDefault(m => m.ProductId == productId);
            item?.SetQuantity(quantity);
        }
    }
}