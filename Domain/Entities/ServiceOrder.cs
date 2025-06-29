using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceOrder : BaseEntity
    {
        public int IdOrder { get; set; }
        public int IdVehicle { get; set; }
        public int IdMechanic { get; set; }
        public int IdServiceType { get; set; }
        public int IdState { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string? ClientMessage { get; set; }
        
        // Navigation properties
        public Vehicle? Vehicle { get; set; }
        public User? Mechanic { get; set; }
        public ServiceType? ServiceType { get; set; }
        public State? State { get; set; }
        public ICollection<DetailsDiagnostic>? DetailsDiagnostics { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }
        public ICollection<InventoryDetail>? InventoryDetails { get; set; }
        public Invoice? Invoice { get; set; }
    }
}