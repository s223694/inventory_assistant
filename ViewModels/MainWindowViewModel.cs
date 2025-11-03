using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using inventory_assistant.Models;
using System.Threading.Tasks;

namespace inventory_assistant.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public OrderBook OrderBook { get; }
        public Inventory Inventory { get; }

        private decimal _totalRevenue;
        private string _statusMessages = "";

        public string StatusMessages
        {
            get => _statusMessages;
            set
            {
                if (_statusMessages != value)
                {
                    _statusMessages = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                if (_totalRevenue != value)
                {
                    _totalRevenue = value;
                    OnPropertyChanged();
                    Console.WriteLine($"[DEBUG] TotalRevenue updated: {_totalRevenue}");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MainWindowViewModel()
        {
            Inventory = new Inventory();
            OrderBook = new OrderBook(Inventory);

            var hydralic_pump = new UnitItem("Hydralic Pump", 4.0m, 0.15m) { InventoryLocation = 1 };
            var screws = new BulkItem("Screws", 3.50m, "kg") { InventoryLocation = 2 };
            var plc_module = new UnitItem("PLC Module", 0.80m, 0.2m) { InventoryLocation = 3 };
            var servo_motor = new UnitItem("Servo Motor", 15.0m, 1.5m) { InventoryLocation = 1 };

            Inventory.AddItem(hydralic_pump, 9);
            Inventory.AddItem(screws, 50);
            Inventory.AddItem(plc_module, 7);
            Inventory.AddItem(servo_motor, 4);

            var customer1 = new Customer("Alice");
            var customer2 = new Customer("Bob");

            var order1 = new Order(customer1.Name, new[]
            {
                new OrderLine(hydralic_pump, 1),
                new OrderLine(screws, 1)
            });

            var order2 = new Order(customer2.Name, new[]
            {
                new OrderLine(plc_module, 1)
            });

            customer1.CreateOrder(OrderBook, order1);
            customer2.CreateOrder(OrderBook, order2);

            Console.WriteLine($"Queued orders at startup: {OrderBook.QueuedOrders.Count}");
        }

        public async void ProcessNextOrder()
        {
            StatusMessages += "Processing order..." + Environment.NewLine;
            Console.WriteLine("Processing order...");

            var robot = new ItemSorterRobot();

            var orderLines = OrderBook.GetNextOrderLines();
            if (orderLines.Count == 0)
            {
                StatusMessages += "No orders left or not enough stock." + Environment.NewLine;
                return;
            }

            decimal orderRevenue = 0;

            foreach (var orderLine in orderLines)
            {
                for (var i = 0; i < orderLine.Quantity; ++i)
                {
                    var name = orderLine.Item.Name;
                    var location = orderLine.Item.InventoryLocation;
                    StatusMessages += $"Picking up {name}" + Environment.NewLine;
                    Console.WriteLine($"[ROBOT] Picking up {name} (Location {location})");

                    robot.PickUp(location);
                    await Task.Delay(9500);
                }

                orderRevenue += orderLine.Item.PricePerUnit * orderLine.Quantity;
            }

            if (orderRevenue > 0)
            {
                TotalRevenue += orderRevenue;
                StatusMessages += $"Order completed. Revenue +{orderRevenue:C}. Total: {TotalRevenue:C}" + Environment.NewLine;
            }
            else
            {
                StatusMessages += "Order completed, no revenue (possibly empty order)." + Environment.NewLine;
            }

            StatusMessages += "Ready for next order." + Environment.NewLine;
            await Task.Delay(3000);
        }
    }
}
