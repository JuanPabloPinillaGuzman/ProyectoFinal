using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ServiceOrderDto
    {
        public int Id { get; set; }
        public int IdVehicle { get; set; }
        public int IdUser { get; set; }
        public int IdServiceType { get; set; }
        public int IdState { get; set; }
        public DateOnly EntryDate { get; set; }
        public DateOnly ExitDate { get; set; }
        public string? ClientMessage { get; set; }
    }
}