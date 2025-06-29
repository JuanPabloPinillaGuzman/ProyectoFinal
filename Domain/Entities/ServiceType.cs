using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceType : BaseEntity
    {
        public int IdServiceType { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<ServiceOrder>? ServiceOrders { get; set; }
    }
}