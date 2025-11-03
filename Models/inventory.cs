using inventory_assistant.Models;
using System.Collections.Generic;
using System.Linq;


namespace inventory_assistant.Models
{
    public class Inventory
    {
   
        private readonly Dictionary<Item, int> stock = new Dictionary<Item, int>();

        public void AddItem(Item item, int amount)
        {
            if (stock.ContainsKey(item))
                stock[item] += amount;
            else
                stock[item] = amount;
        }

        public int GetQuantity(Item item)
        {
            return stock.ContainsKey(item) ? stock[item] : 0;
        }
         public bool CanFulfill(IEnumerable<OrderLine> orderLines)
        {
            foreach (var line in orderLines)
            {
                if (!stock.ContainsKey(line.Item))
                    return false;

                if (stock[line.Item] < line.Quantity)
                    return false;
            }

            return true;
        }

   
        public void Consume(IEnumerable<OrderLine> orderLines)
        {
            foreach (var line in orderLines)
            {
                if (stock.ContainsKey(line.Item))
                {
                    stock[line.Item] -= (int)line.Quantity;
                }
            }
        }     

        
        

 
        public List<Item> LowStockItems()
        {
            return stock
                .Where(pair => pair.Value < 5)
                .Select(pair => pair.Key)
                .ToList();
        }
     
    }
}