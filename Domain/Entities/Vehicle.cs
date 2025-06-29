using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public int Id { get; set; }
        public int IdClient { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? SerialNumberVIN { get; set; }
        public int Mileage { get; set; }
        
        // Navigation properties
        public Client? Client { get; set; }
        public ICollection<ServiceOrder>? ServiceOrders { get; set; }
    }
}