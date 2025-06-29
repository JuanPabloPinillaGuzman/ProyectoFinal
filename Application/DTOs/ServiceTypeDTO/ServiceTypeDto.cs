using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ServiceTypeDto
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public string? Description { get; set; }
    }
}