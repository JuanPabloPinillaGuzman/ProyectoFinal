using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserRole
    {
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        
        // Navigation properties
        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}