using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class State : BaseEntity
    {
        public int Id { get; set; }
        public string? StateType { get; set; }

        // Navigation properties
        public ICollection<ServiceOrder>? ServiceOrders { get; set; }
    }
}