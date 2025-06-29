using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class CreateServiceOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateServiceOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceOrderDto> CreateServiceOrderAsync(CreateServiceOrderDto serviceOrderDto)
        {
            // 1. Validar que el DTO no sea nulo
            if (serviceOrderDto == null)
                throw new ArgumentNullException(nameof(serviceOrderDto), "Service order data cannot be null");

            // 2. Validar que el vehículo existe
            var vehicle = await _unitOfWork.Vehicle.GetByIdAsync(serviceOrderDto.VehicleId);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle with id {serviceOrderDto.VehicleId} not found");

            // 3. Validar que el tipo de servicio existe
            var serviceType = await _unitOfWork.ServiceType.GetByIdAsync(serviceOrderDto.ServiceTypeId);
            if (serviceType == null)
                throw new KeyNotFoundException($"Service Type with id {serviceOrderDto.ServiceTypeId} not found");

            // 4. Validar que el estado existe
            var state = await _unitOfWork.State.GetByIdAsync(serviceOrderDto.StateId);
            if (state == null)
                throw new KeyNotFoundException($"State with id {serviceOrderDto.StateId} not found");

            // 5. Crear la orden de servicio
            var serviceOrder = new ServiceOrder
            {
                IdVehicle = serviceOrderDto.VehicleId,
                IdUser = 1, 
                IdServiceType = serviceOrderDto.ServiceTypeId,
                IdState = serviceOrderDto.StateId,
                EntryDate = serviceOrderDto.EntryDate,
                ExitDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)), 
                ClientMessage = serviceOrderDto.ClientMessage
            };

            // 6. Guardar la orden de servicio
            _unitOfWork.ServiceOrder.Add(serviceOrder);
            await _unitOfWork.SaveAsync();

            // 7. Procesar los detalles de la orden (repuestos)
            if (serviceOrderDto.OrderDetails != null && serviceOrderDto.OrderDetails.Any())
            {
                foreach (var detailDto in serviceOrderDto.OrderDetails)
                {
                    // Validar que el repuesto existe
                    var replacement = await _unitOfWork.Replacement.GetByIdAsync(detailDto.IdReplacement);
                    if (replacement == null)
                        throw new KeyNotFoundException($"Replacement with id {detailDto.IdReplacement} not found");

                    // Validar stock disponible
                    if (replacement.StockQuantity < detailDto.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for {replacement.Description}. Available: {replacement.StockQuantity}, Required: {detailDto.Quantity}");

                    // Actualizar stock
                    replacement.StockQuantity -= detailDto.Quantity;
                    _unitOfWork.Replacement.Update(replacement);

                    // Crear el detalle de la orden
                    var orderDetail = new OrderDetails
                    {
                        IdOrder = serviceOrder.Id,
                        IdReplacement = detailDto.IdReplacement,
                        Quantity = detailDto.Quantity,
                        TotalCost = replacement.UnitPrice * detailDto.Quantity
                    };

                    _unitOfWork.OrderDetails.Add(orderDetail);
                }
            }

            // 8. Guardar todos los cambios
            await _unitOfWork.SaveAsync();

            // 9. Obtener la orden de servicio creada con sus relaciones
            var createdServiceOrder = await _unitOfWork.ServiceOrder.GetByIdAsync(serviceOrder.Id);

            // 10. Mapear a DTO y retornar
            var serviceOrderDto = new ServiceOrderDto
            {
                Id = createdServiceOrder.Id,
                IdVehicle = createdServiceOrder.IdVehicle,
                IdUser = createdServiceOrder.IdUser,
                IdServiceType = createdServiceOrder.IdServiceType,
                IdState = createdServiceOrder.IdState,
                EntryDate = createdServiceOrder.EntryDate,
                ExitDate = createdServiceOrder.ExitDate,
                ClientMessage = createdServiceOrder.ClientMessage
            };

            // 11. Log de la orden creada (opcional)
            Console.WriteLine($"Service Order created successfully:");
            Console.WriteLine($"- Service Order ID: {serviceOrder.Id}");
            Console.WriteLine($"- Vehicle ID: {serviceOrder.IdVehicle}");
            Console.WriteLine($"- Service Type ID: {serviceOrder.IdServiceType}");
            Console.WriteLine($"- State ID: {serviceOrder.IdState}");
            Console.WriteLine($"- Entry Date: {serviceOrder.EntryDate}");
            Console.WriteLine($"- Order Details Count: {serviceOrderDto.OrderDetails?.Count ?? 0}");

            return serviceOrderDto;
        }
    }
}