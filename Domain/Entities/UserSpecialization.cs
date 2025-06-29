using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserSpecialization
    {
        public int IdUser { get; set; }
        public int IdSpecialization { get; set; }
        
        // Navigation properties
        public User? User { get; set; }
        public Specialization? Specialization { get; set; }
    }
}