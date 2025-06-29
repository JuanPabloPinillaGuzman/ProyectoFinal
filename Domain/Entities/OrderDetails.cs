using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderDetails : BaseEntity
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdReplacement { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        
        // Navigation properties
        public ServiceOrder? ServiceOrder { get; set; }
        public Replacement? Replacement { get; set; }
    }
}