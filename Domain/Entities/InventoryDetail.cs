using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InventoryDetail : BaseEntity
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdInventory { get; set; }
        public int Quantity { get; set; }
        
        // Navigation properties
        public ServiceOrder? ServiceOrder { get; set; }
        public Inventory? Inventory { get; set; }
    }
}