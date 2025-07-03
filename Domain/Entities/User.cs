using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<UserSpecialization>? UserSpecializations { get; set; }
        public ICollection<ServiceOrder>? ServiceOrders { get; set; }
        public ICollection<Diagnostic>? Diagnostics { get; set; }
        public ICollection<Auditory>? AuditoryRecords { get; set; }
        public ICollection<Role>? Roles { get; set; } = new HashSet<Role>();
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}