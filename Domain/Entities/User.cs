using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public int IdUser { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        // Navigation properties
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<UserSpecialization>? UserSpecializations { get; set; }
        public ICollection<ServiceOrder>? ServiceOrders { get; set; }
        public ICollection<Diagnostic>? Diagnostics { get; set; }
        public ICollection<Auditory>? AuditoryRecords { get; set; }
    }
}