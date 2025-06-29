using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDetailsDiagnosticRepository
    {
        Task<IEnumerable<DetailsDiagnostic>> GetAllAsync();
        void Remove(DetailsDiagnostic entity);
        void Update(DetailsDiagnostic entity);
        Task<DetailsDiagnostic?> GetByIdsAsync(int diagnosticId, int serviceOrderId);
    }
}