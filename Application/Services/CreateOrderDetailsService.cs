using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class CreateOrderDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderDetailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrderDetailDto>> CreateOrderDetailsAsync(int serviceOrderId, IEnumerable<OrderDetailDto> orderDetailsDto)
        {
            // 1. Validar que la colección no sea nula
            if (orderDetailsDto == null || !orderDetailsDto.Any())
                throw new ArgumentException("Order details collection cannot be null or empty", nameof(orderDetailsDto));

            // 2. Validar que la orden de servicio existe
            var serviceOrder = await _unitOfWork.ServiceOrder.GetByIdAsync(serviceOrderId);
            if (serviceOrder == null)
                throw new KeyNotFoundException($"Service Order with id {serviceOrderId} not found");

            // 3. Obtener todos los repuestos necesarios de una vez
            var replacementIds = orderDetailsDto.Select(od => od.IdReplacement).Distinct().ToList();
            var replacements = new List<Replacement>();

            foreach (var replacementId in replacementIds)
            {
                var replacement = await _unitOfWork.Replacement.GetByIdAsync(replacementId);
                if (replacement == null)
                    throw new KeyNotFoundException($"Replacement with id {replacementId} not found");
                replacements.Add(replacement);
            }

            // 4. Validar stock para todos los repuestos antes de procesar
            var stockValidationErrors = new List<string>();
            foreach (var orderDetail in orderDetailsDto)
            {
                var replacement = replacements.First(r => r.Id == orderDetail.IdReplacement);
                if (replacement.StockQuantity < orderDetail.Quantity)
                {
                    stockValidationErrors.Add($"Insufficient stock for {replacement.Description}. Available: {replacement.StockQuantity}, Required: {orderDetail.Quantity}");
                }
            }

            if (stockValidationErrors.Any())
                throw new InvalidOperationException($"Stock validation failed: {string.Join("; ", stockValidationErrors)}");

            // 5. Crear todos los detalles de orden
            var orderDetails = new List<OrderDetails>();
            var createdOrderDetails = new List<OrderDetailDto>();

            foreach (var orderDetailDto in orderDetailsDto)
            {
                var replacement = replacements.First(r => r.Id == orderDetailDto.IdReplacement);
                var totalCost = replacement.UnitPrice * orderDetailDto.Quantity;

                var orderDetail = new OrderDetails
                {
                    IdOrder = serviceOrderId,
                    IdReplacement = orderDetailDto.IdReplacement,
                    Quantity = orderDetailDto.Quantity,
                    TotalCost = totalCost
                };

                orderDetails.Add(orderDetail);

                // Actualizar stock
                replacement.StockQuantity -= orderDetailDto.Quantity;
                _unitOfWork.Replacement.Update(replacement);

                // Agregar a la lista de resultados
                createdOrderDetails.Add(new OrderDetailDto
                {
                    IdReplacement = orderDetailDto.IdReplacement,
                    Quantity = orderDetailDto.Quantity
                });
            }

            // 6. Guardar todos los detalles de orden
            foreach (var orderDetail in orderDetails)
            {
                _unitOfWork.OrderDetails.Add(orderDetail);
            }

            // 7. Guardar todos los cambios
            await _unitOfWork.SaveAsync();

            // 8. Log de los detalles creados (opcional)
            Console.WriteLine($"Order Details created successfully:");
            Console.WriteLine($"- Service Order ID: {serviceOrderId}");
            Console.WriteLine($"- Total Details Created: {orderDetails.Count}");
            Console.WriteLine($"- Total Cost: ${orderDetails.Sum(od => od.TotalCost)}");

            foreach (var detail in orderDetails)
            {
                var replacement = replacements.First(r => r.Id == detail.IdReplacement);
                Console.WriteLine($"  - {replacement.Description}: {detail.Quantity} units @ ${replacement.UnitPrice} = ${detail.TotalCost}");
            }

            return createdOrderDetails;
        }
    }
} 