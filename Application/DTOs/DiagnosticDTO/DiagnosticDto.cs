using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class Diagnostic
    {
        public int IdDiagnostic { get; set; }
        public int IdUser { get; set; }
        public string? Description { get; set; }
    }
}