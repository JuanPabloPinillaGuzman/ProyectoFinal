using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public int IdInventory { get; set; }
        public string? Name { get; set; }

        // Navigation properties
        public ICollection<InventoryDetail>? InventoryDetails { get; set; }
    }
}