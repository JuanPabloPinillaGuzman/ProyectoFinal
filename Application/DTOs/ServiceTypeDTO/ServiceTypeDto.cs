using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ServiceType
    {
        public int IdServiceType { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
    }
}