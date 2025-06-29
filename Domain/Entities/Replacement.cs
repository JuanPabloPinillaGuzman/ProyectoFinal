using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Replacement : BaseEntity
    {
        public int IdReplacement { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public int? MinimumStock { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Category { get; set; }

        // Navigation properties
        public ICollection<OrderDetails>? OrderDetails { get; set; }
    }
}