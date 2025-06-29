using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuditoryDto
    {
        public int Id { get; set; }
        public string? EntityName { get; set; }
        public string? ChangeType { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime Date { get; set; }
    }
}