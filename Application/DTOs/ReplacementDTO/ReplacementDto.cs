using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReplacementDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public int? MinimumStock { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Category { get; set; }
    }
}