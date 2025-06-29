using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public int IdInvoice { get; set; }
        public int IdOrder { get; set; }
        public DateOnly IssueDate { get; set; }
        public decimal LaborTotal { get; set; }
        public decimal ReplacementsTotal { get; set; }
        public decimal TotalAmount { get; set; }
        
        // Navigation properties
        public ServiceOrder? ServiceOrder { get; set; }
    }
}