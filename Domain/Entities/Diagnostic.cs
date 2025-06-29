using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Diagnostic : BaseEntity
    {
        public int IdDiagnostic { get; set; }
        public int IdUser { get; set; }
        public string? Description { get; set; }
        
        // Navigation properties
        public User? User { get; set; }
        public ICollection<DetailsDiagnostic>? DetailsDiagnostics { get; set; }
    }
}