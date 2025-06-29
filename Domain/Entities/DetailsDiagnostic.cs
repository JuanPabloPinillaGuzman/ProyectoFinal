using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DetailsDiagnostic
    {
        public int IdServiceOrder { get; set; }
        public int IdDiagnostic { get; set; }
        
        // Navigation properties
        public ServiceOrder? ServiceOrder { get; set; }
        public Diagnostic? Diagnostic { get; set; }
    }
}