using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.WebApi
{
    public class Cart
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public Guid Id { get; }

        public IReadOnlyCollection<CartItem> Items => _items;

        public void Add(int productId, int quantity = 1)
        {
            var existingItem = _items.FirstOrDefault(m => m.ProductId == productId);
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
            _items.Add(newItem);
        }

        public void DeleteItemBy(int productId)
        {
            var item = _items.FirstOrDefault(m => m.ProductId == productId);
            if (item != null)
                _items.Remove(item);
        }

        public void ClearAllItems()
        {
            _items.Clear();
        }

        public void UpdateItem(int productId, int quantity)
        {
            var item = _items.FirstOrDefault(m => m.ProductId == productId);
            if (item != null)
                item.SetQuantity(quantity);
        }
    }
}