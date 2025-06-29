using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace ApiProject.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auditory, AuditoryDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<DetailsDiagnostic, DetailsDiagnosticDto>().ReverseMap();
            CreateMap<Diagnostic, DiagnosticDto>().ReverseMap();
            CreateMap<InventoryDetail, InventoryDetailDto>().ReverseMap();
            CreateMap<Inventory, IventoryDto>().ReverseMap();
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
            CreateMap<Replacement, ReplacementDto>().ReverseMap();            
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<ServiceOrder, ServiceOrderDto>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeDto>().ReverseMap();
            CreateMap<Specialization, SpecializationDto>().ReverseMap();
            CreateMap<State, StateDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRole, UserRoleDto>().ReverseMap();
            CreateMap<UserSpecialization, UserSpecializationDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
        }
    }
}