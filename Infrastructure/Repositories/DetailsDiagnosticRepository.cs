using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DetailsDiagnosticRepository : IDetailsDiagnosticRepository
    {
        public readonly AutoTallerDbContext _context;

        public DetailsDiagnosticRepository(AutoTallerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetailsDiagnostic>> GetAllAsync()
        {
            return await _context.Set<DetailsDiagnostic>().ToListAsync();
        }
        public async Task<DetailsDiagnostic?> GetByIdsAsync(int diagnosticId, int serviceOrderId)
        {
            return await _context.DetailsDiagnostic
                .FirstOrDefaultAsync(ur => ur.IdDiagnostic == diagnosticId && ur.IdServiceOrder == serviceOrderId)
                ?? throw new KeyNotFoundException($"Details diagnostic with DiagnosticId {diagnosticId} and ServiceOrderId {serviceOrderId} was not found");
        }

        public void Remove(DetailsDiagnostic entity)
        {
            _context.Set<DetailsDiagnostic>().Remove(entity);
        }

        public void Update(DetailsDiagnostic entity)
        {
            _context.Set<DetailsDiagnostic>()
                .Update(entity);
        }
    }
}