using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.DTOs
{
    public class CreateServiceOrderDto
    {
        public int VehicleId { get; set; }
        public int ServiceTypeId { get; set; }
        public int ClientId { get; set; }
        public int StateId { get; set; }
        public DateOnly EntryDate { get; set; }
        public string? ClientMessage { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }

    public class OrderDetailDto
    {
        public int IdReplacement { get; set; }
        public int Quantity { get; set; }
    }
}
