using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ServiceOrder
    {
        public int IdOrder { get; set; }
        public int IdVehicle { get; set; }
        public int IdMechanic { get; set; }
        public int IdServiceType { get; set; }
        public int IdState { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string? ClientMessage { get; set; }
    }
}