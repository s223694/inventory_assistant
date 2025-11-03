

namespace inventory_assistant.Models
{

    public class Item
    {
        public string Name { get; }
        public decimal PricePerUnit { get; }

        public uint InventoryLocation { get; set; }

        public Item(string name, decimal pricePerUnit)
        {
            Name = name;
            PricePerUnit = pricePerUnit;
            
        }   
          public override string ToString()
        {
            return $"{Name}: {PricePerUnit} per unit";
        }
        }    
    
}