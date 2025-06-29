using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateOrderDetailsController : BaseApiController
    {
        private readonly ICreateOrderDetailsService _createOrderDetailsService;

        public CreateOrderDetailsController(ICreateOrderDetailsService createOrderDetailsService)
        {
            _createOrderDetailsService = createOrderDetailsService;
        }

        /// <summary>
        /// Crea múltiples detalles de orden para una orden de servicio específica
        /// </summary>
        /// <param name="serviceOrderId">ID de la orden de servicio</param>
        /// <param name="orderDetailsDto">Lista de detalles de orden a crear</param>
        /// <returns>Lista de detalles de orden creados</returns>
        [HttpPost("service-order/{serviceOrderId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> CreateOrderDetails(
            [FromRoute] int serviceOrderId,
            [FromBody] IEnumerable<OrderDetailDto> orderDetailsDto)
        {
            try
            {
                if (orderDetailsDto == null || !orderDetailsDto.Any())
                {
                    return BadRequest("Order details collection cannot be null or empty");
                }

                // Validar que los datos de entrada sean válidos
                var validationErrors = new List<string>();
                foreach (var detail in orderDetailsDto)
                {
                    if (detail.IdReplacement <= 0)
                        validationErrors.Add("Replacement ID must be greater than 0");
                    if (detail.Quantity <= 0)
                        validationErrors.Add("Quantity must be greater than 0");
                }

                if (validationErrors.Any())
                {
                    return BadRequest(new { errors = validationErrors });
                }

                var createdOrderDetails = await _createOrderDetailsService.CreateOrderDetailsAsync(serviceOrderId, orderDetailsDto);
                
                return CreatedAtAction(
                    nameof(GetOrderDetailsByServiceOrder), 
                    new { serviceOrderId }, 
                    createdOrderDetails);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while creating order details" });
            }
        }

        /// <summary>
        /// Obtiene todos los detalles de orden para una orden de servicio específica
        /// </summary>
        /// <param name="serviceOrderId">ID de la orden de servicio</param>
        /// <returns>Lista de detalles de orden</returns>
        [HttpGet("service-order/{serviceOrderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetailsByServiceOrder(int serviceOrderId)
        {
            try
            {
                // Este método requeriría implementación adicional en el servicio
                // Por ahora, retornamos un mensaje indicando que no está implementado
                return NotFound(new { message = "This endpoint is not yet implemented" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred while retrieving order details" });
            }
        }
    }
} 