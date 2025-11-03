using System;

namespace inventory_assistant.Models
{
    public class ItemSorterRobot : Robot
    {
        public const string UrscriptTemplate = @"
def move_item_to_shipment_box():
    SBOX_X = 3
    SBOX_Y = 3
    ITEM_X = {0}
    ITEM_Y = 1

    def moveto(x, y, z = 0.3):
        pose = p[x * 0.1, y * 0.1, z, 0, -3.1415, 0]
        movej(get_inverse_kin(pose))
    end

    moveto(ITEM_X, ITEM_Y, 0.3)
    moveto(ITEM_X, ITEM_Y, 0.2)
    moveto(ITEM_X, ITEM_Y, 0.3)
    moveto(SBOX_X, SBOX_Y, 0.3)
    moveto(SBOX_X, SBOX_Y, 0.2)
    moveto(SBOX_X, SBOX_Y, 0.3)
end

move_item_to_shipment_box()
";

        private static int MapSlotToItemX(uint slot) => slot switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(slot), "InventoryLocation skal være 1..3")
        };

        public void PickUp(uint itemLocation)
        {
            var x = MapSlotToItemX(itemLocation);
            var program = string.Format(UrscriptTemplate, x);
            SendUrScript(program);
        }
    }
}
