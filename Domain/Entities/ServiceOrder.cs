using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceOrder : BaseEntity
    {
        public int Id { get; set; }
        public int IdVehicle { get; set; }
        public int IdUser { get; set; }
        public int IdServiceType { get; set; }
        public int IdState { get; set; }
        public DateOnly EntryDate { get; set; }
        public DateOnly ExitDate { get; set; }
        public string? ClientMessage { get; set; }
        
        // Navigation properties
        public Vehicle? Vehicle { get; set; }
        public User? User { get; set; }
        public ServiceType? ServiceType { get; set; }
        public State? State { get; set; }
        public ICollection<DetailsDiagnostic>? DetailsDiagnostics { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }
        public ICollection<InventoryDetail>? InventoryDetails { get; set; }
        public Invoice? Invoice { get; set; }
    }
}