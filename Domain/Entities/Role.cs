using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public int IdRole { get; set; }
        public string? Description { get; set; }
        
        // Navigation properties
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}