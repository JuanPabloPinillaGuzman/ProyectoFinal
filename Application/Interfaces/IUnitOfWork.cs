using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAuditoryRepository Auditory { get; }
        IClientRepository Client { get; }
        IDetailsDiagnosticRepository DetailsDiagnostic { get; }
        IDiagnosticRepository Diagnostic { get; }
        IInventoryDetailRepository InventoryDetail { get; }
        IInventoryRepository Inventory { get; }
        IInvoiceRepository Invoice { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IReplacementRepository Replacement { get; }
        IRoleRepository Role { get; }
        IServiceOrderRepository ServiceOrder { get; }
        IServiceTypeRepository ServiceType { get; }
        IStateRepository State { get; }
        ISpecializationRepository Specialization { get; }
        IUserRepository User { get; }
        IUserRoleRepository UserRole { get; }
        IUserSpecializationRepository UserSpecialization { get; }
        IVehicleRepository Vehicle { get; }
        IRefreshTokenRepository RefreshToken { get; }
        Task<int> SaveAsync();
    }
}