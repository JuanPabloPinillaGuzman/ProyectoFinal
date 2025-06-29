using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdReplacement { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
    }
}