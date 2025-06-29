using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class InventoryDetail
    {
        public int IdDetail { get; set; }
        public int IdOrder { get; set; }
        public int IdInventory { get; set; }
        public int Quantity { get; set; }
    }
}