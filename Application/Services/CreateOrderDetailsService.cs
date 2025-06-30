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

        public async Task CreateOrderDetailsAsync(int idServiceOrder, int IdReplacement, int quantity)
        {
            var serviceOrder = await _unitOfWork.ServiceOrder.GetByIdAsync(idServiceOrder);
            if (serviceOrder == null)
            {
                throw new Exception($"Service Order with id {idServiceOrder} not found");
            }

            var replacement = await _unitOfWork.Replacement.GetByIdAsync(IdReplacement);
            if (replacement == null)
            {
                throw new Exception($"Spare part with id {IdReplacement} not found");
            }

            if (replacement.StockQuantity < quantity)
            {
                throw new Exception($"Insufficient stock for spare part {replacement.Description}. Available: {replacement.StockQuantity}, Required: {quantity}");
            }

            var totalPrice = replacement.UnitPrice * quantity;

            var orderDetail = new OrderDetails
            {
                IdOrder = idServiceOrder,
                IdReplacement = IdReplacement,
                Quantity = quantity,
                TotalCost = totalPrice
            };

            _unitOfWork.OrderDetails.Add(orderDetail);

            replacement.StockQuantity -= quantity;
            _unitOfWork.Replacement.Update(replacement);

            await _unitOfWork.SaveAsync();
        }
    }
} 