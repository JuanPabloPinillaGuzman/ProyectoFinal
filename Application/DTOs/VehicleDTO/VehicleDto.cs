using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public int IdClient { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? SerialNumberVIN { get; set; }
        public int Mileage { get; set; }
    }
}