using System;
using System.Collections.Generic;              // List<T>
using System.Collections.ObjectModel;
using System.Linq;                             // .First(), .ToList()

namespace inventory_assistant.Models
{
    public class OrderBook
    {
        public ObservableCollection<Order> QueuedOrders { get; } = new();
        public ObservableCollection<Order> ProcessedOrders { get; } = new();

        private readonly Inventory _inventory;

        public OrderBook(Inventory inventory)
        {
            _inventory = inventory;
        }

        public void QueueOrder(Order order)
        {
            QueuedOrders.Add(order);
        }

        // Keep your original method (used elsewhere for revenue)
        public decimal ProcessNextOrder()
        {
            if (QueuedOrders.Count == 0)
                return 0;

            var order = QueuedOrders.First();

            if (!_inventory.CanFulfill(order.OrderLines))
                return 0;

            _inventory.Consume(order.OrderLines);

            QueuedOrders.Remove(order);
            ProcessedOrders.Insert(0, order);

            return order.TotalPrice;
        }


        public List<OrderLine> GetNextOrderLines()
        {
            if (QueuedOrders.Count == 0)
                return new List<OrderLine>();

            var order = QueuedOrders.First();

            if (!_inventory.CanFulfill(order.OrderLines))
                return new List<OrderLine>();

            _inventory.Consume(order.OrderLines);
            QueuedOrders.Remove(order);
            ProcessedOrders.Insert(0, order);

           
            return order.OrderLines.ToList();
        }
    }
}
