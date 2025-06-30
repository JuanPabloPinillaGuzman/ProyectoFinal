using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AutoTallerDbContext _context;
        private IAuditoryRepository? _auditory;
        private IClientRepository? _client;
        private IDetailsDiagnosticRepository? _detailsDiagnostic;
        private IDiagnosticRepository? _diagnostic;
        private IInventoryDetailRepository? _inventoryDetail;
        private IInventoryRepository? _inventory;
        private IInvoiceRepository? _invoice;
        private IOrderDetailsRepository? _orderDetails;
        private IReplacementRepository? _replacement;
        private IRoleRepository? _role;
        private IServiceOrderRepository? _serviceOrder;
        private IServiceTypeRepository? _serviceType;
        private IStateRepository? _state;
        private ISpecializationRepository? _specialization;
        private IUserRepository? _user;
        private IUserRoleRepository? _userRole;
        private IUserSpecializationRepository? _userSpecialization;
        private IVehicleRepository? _vehicle;
        private IRefreshTokenRepository? _refreshToken;

        public UnitOfWork(AutoTallerDbContext context)
        {
            _context = context;
        }

        public IAuditoryRepository Auditory
        {
            get
            {
                if (_auditory == null)
                {
                    _auditory = new AuditoryRepository(_context);
                }
                return _auditory;
            }
        }

        public IClientRepository Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new ClientRepository(_context);
                }
                return _client;
            }
        }

        public IDetailsDiagnosticRepository DetailsDiagnostic
        {
            get
            {
                if (_detailsDiagnostic == null)
                {
                    _detailsDiagnostic = new DetailsDiagnosticRepository(_context);
                }
                return _detailsDiagnostic;
            }
        }
        public IDiagnosticRepository Diagnostic
        {
            get
            {
                if (_diagnostic == null)
                {
                    _diagnostic = new DiagnosticRepository(_context);
                }
                return _diagnostic;
            }
        }

        public IInventoryDetailRepository InventoryDetail
        {
            get
            {
                if (_inventoryDetail == null)
                {
                    _inventoryDetail = new InventoryDetailRepository(_context);
                }
                return _inventoryDetail;
            }
        }

        public IInventoryRepository Inventory
        {
            get
            {
                if (_inventory == null)
                {
                    _inventory = new InventoryRepository(_context);
                }
                return _inventory;
            }
        }

        public IInvoiceRepository Invoice
        {
            get
            {
                if (_invoice == null)
                {
                    _invoice = new InvoiceRepository(_context);
                }
                return _invoice;
            }
        }

        public IOrderDetailsRepository OrderDetails
        {
            get
            {
                if (_orderDetails == null)
                {
                    _orderDetails = new OrderDetailsRepository(_context);
                }
                return _orderDetails;
            }
        }

        public IReplacementRepository Replacement
        {
            get
            {
                if (_replacement == null)
                {
                    _replacement = new ReplacementRepository(_context);
                }
                return _replacement;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_context);
                }
                return _role;
            }
        }

        public IServiceOrderRepository ServiceOrder
        {
            get
            {
                if (_serviceOrder == null)
                {
                    _serviceOrder = new ServiceOrderRepository(_context);
                }
                return _serviceOrder;
            }
        }

        public IServiceTypeRepository ServiceType
        {
            get
            {
                if (_serviceType == null)
                {
                    _serviceType = new ServiceTypeRepository(_context);
                }
                return _serviceType;
            }
        }

        public IStateRepository State
        {
            get
            {
                if (_state == null)
                {
                    _state = new StateRepository(_context);
                }
                return _state;
            }
        }

        public ISpecializationRepository Specialization
        {
            get
            {
                if (_specialization == null)
                {
                    _specialization = new SpecializationRepository(_context);
                }
                return _specialization;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }

        public IUserRoleRepository UserRole
        {
            get
            {
                if (_userRole == null)
                {
                    _userRole = new UserRoleRepository(_context);
                }
                return _userRole;
            }
        }

        public IUserSpecializationRepository UserSpecialization
        {
            get
            {
                if (_userSpecialization == null)
                {
                    _userSpecialization = new UserSpecializationRepository(_context);
                }
                return _userSpecialization;
            }
        }

        public IVehicleRepository Vehicle
        {
            get
            {
                if (_vehicle == null)
                {
                    _vehicle = new VehicleRepository(_context);
                }
                return _vehicle;
            }
        }

        public IRefreshTokenRepository RefreshToken
        {
            get
            {
                if (_refreshToken == null)
                {
                    _refreshToken = new RefreshTokenRepository(_context);
                }
                return _refreshToken;
            }
        }

         public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}