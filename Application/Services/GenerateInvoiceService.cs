using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class GenerateInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenerateInvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<InvoiceDto> GenerateInvoiceAsync(int serviceOrderId, decimal laborCost = 0)
        {
            // 1. Validar que la orden de servicio existe
            var serviceOrder = await _unitOfWork.ServiceOrder.GetByIdAsync(serviceOrderId);
            if (serviceOrder == null)
                throw new KeyNotFoundException($"Service Order with id {serviceOrderId} not found");

            // 2. Verificar que la orden de servicio no tenga factura ya generada
            var existingInvoice = await _unitOfWork.Invoice.GetAllAsync(1, 1000, "")
                .ContinueWith(t => t.Result.registers.FirstOrDefault(i => i.IdServiceOrder == serviceOrderId));
            
            if (existingInvoice != null)
                throw new InvalidOperationException($"Invoice already exists for Service Order {serviceOrderId}");

            // 3. Obtener los detalles de la orden (repuestos utilizados)
            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync(1, 1000, "")
                .ContinueWith(t => t.Result.registers.Where(od => od.IdOrder == serviceOrderId).ToList());

            // 4. Calcular el total de repuestos
            decimal replacementsTotal = 0;
            var replacementDetails = new List<object>();

            foreach (var detail in orderDetails)
            {
                var replacement = await _unitOfWork.Replacement.GetByIdAsync(detail.IdReplacement);
                if (replacement == null)
                    throw new KeyNotFoundException($"Replacement with id {detail.IdReplacement} not found");

                var detailTotal = replacement.UnitPrice * detail.Quantity;
                replacementsTotal += detailTotal;

                replacementDetails.Add(new
                {
                    ReplacementId = replacement.Id,
                    ReplacementCode = replacement.Code,
                    ReplacementDescription = replacement.Description,
                    Quantity = detail.Quantity,
                    UnitPrice = replacement.UnitPrice,
                    TotalCost = detailTotal
                });
            }

            // 5. Calcular el total general
            decimal totalAmount = laborCost + replacementsTotal;

            // 6. Crear la factura
            var invoice = new Invoice
            {
                IdServiceOrder = serviceOrderId,
                IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
                LaborTotal = laborCost,
                ReplacementsTotal = replacementsTotal,
                TotalAmount = totalAmount
            };

            // 7. Guardar la factura
            _unitOfWork.Invoice.Add(invoice);
            await _unitOfWork.SaveAsync();

            // 8. Actualizar el estado de la orden de servicio a "Facturado" (asumiendo que el estado 3 es facturado)
            serviceOrder.IdState = 3; // Estado facturado
            _unitOfWork.ServiceOrder.Update(serviceOrder);
            await _unitOfWork.SaveAsync();

            // 9. Obtener la factura creada con sus relaciones
            var createdInvoice = await _unitOfWork.Invoice.GetByIdAsync(invoice.Id);
            
            // 10. Mapear manualmente a DTO
            var invoiceDto = new InvoiceDto
            {
                Id = createdInvoice.Id,
                IdOrder = createdInvoice.IdServiceOrder,
                IssueDate = createdInvoice.IssueDate.ToDateTime(TimeOnly.MinValue),
                LaborTotal = createdInvoice.LaborTotal,
                ReplacementsTotal = createdInvoice.ReplacementsTotal,
                TotalAmount = createdInvoice.TotalAmount
            };
            
            // 11. Log de la factura generada (opcional)
            Console.WriteLine($"Invoice generated successfully:");
            Console.WriteLine($"- Service Order ID: {serviceOrderId}");
            Console.WriteLine($"- Invoice ID: {invoice.Id}");
            Console.WriteLine($"- Labor Cost: ${laborCost}");
            Console.WriteLine($"- Replacements Total: ${replacementsTotal}");
            Console.WriteLine($"- Total Amount: ${totalAmount}");

            return invoiceDto;
        }
    }
}