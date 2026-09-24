using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryService.Model
{
    public class Inventory
    {
        public Guid ProductId { get; set; }

        public int AvailableQuantity { get; set; }

        public int ReservedQuantity { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
