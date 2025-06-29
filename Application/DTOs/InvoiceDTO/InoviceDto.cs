using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal LaborTotal { get; set; }
        public decimal ReplacementsTotal { get; set; }
        public decimal TotalAmount { get; set; }
    }
}