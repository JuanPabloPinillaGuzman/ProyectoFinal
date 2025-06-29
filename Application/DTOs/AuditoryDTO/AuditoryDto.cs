using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class Auditory
    {
        public int IdAuditory { get; set; }
        public string? AffectedEntity { get; set; }
        public string? ActionType { get; set; }
        public int ResponsibleUserId { get; set; }
        public DateTime Date { get; set; }
    }
}