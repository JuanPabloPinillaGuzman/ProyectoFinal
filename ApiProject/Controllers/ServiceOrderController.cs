using System;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiProject.Controllers;
using Application.DTOs;
using AutoMapper;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator, Recepcionist, Mechanic")]
    public class ServiceOrderController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateOrderDetailsService _registerOrderDetails;

        public ServiceOrderController(IUnitOfWork unitOfWork, IMapper mapper, CreateOrderDetailsService registerOrderDetails)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _registerOrderDetails = registerOrderDetails;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServiceOrderDto>>> Get()
        {
            var serviceOrders = await _unitOfWork.ServiceOrder.GetAllAsync();
            return _mapper.Map<List<ServiceOrderDto>>(serviceOrders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceOrderDto>> Get(int id)
        {
            var serviceOrder = await _unitOfWork.ServiceOrder.GetByIdAsync(id);
            if (serviceOrder == null)
                return NotFound($"ServiceOrder with id {id} was not found.");
            return _mapper.Map<ServiceOrderDto>(serviceOrder);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceOrder>> Post(ServiceOrderDto serviceOrderDto)
        {
            var serviceOrder = _mapper.Map<ServiceOrder>(serviceOrderDto);
            _unitOfWork.ServiceOrder.Add(serviceOrder);
            await _unitOfWork.SaveAsync();
            if (serviceOrder == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = serviceOrder.Id }, serviceOrder);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] ServiceOrderDto serviceOrderDto)
        {
            if (serviceOrderDto == null)
                return NotFound();

            var serviceOrder = _mapper.Map<ServiceOrder>(serviceOrderDto);
            _unitOfWork.ServiceOrder.Update(serviceOrder);
            await _unitOfWork.SaveAsync();
            return Ok(serviceOrder);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceOrder = await _unitOfWork.ServiceOrder.GetByIdAsync(id);
            if (serviceOrder == null)
                return NotFound();
            _unitOfWork.ServiceOrder.Remove(serviceOrder);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServiceOrderDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (allRegisters, registers) = await _unitOfWork.ServiceOrder.GetAllAsync(pageNumber, pageSize, search);
            var serviceOrderDtos = _mapper.Map<List<ServiceOrderDto>>(registers);

            // Agregar X-Total-Count en los encabezados HTTP
            Response.Headers.Append("X-Total-Count", allRegisters.ToString());

            return Ok(serviceOrderDtos);
        }

        [HttpPost("{idServiceOrder}/details")]
        public async Task<IActionResult> AddOrderDetail(int idServiceOrder, [FromBody] OrderDetailsDto orderDetail)
        {
            try
            {
                await _registerOrderDetails.CreateOrderDetailsAsync(
                    idServiceOrder,
                    orderDetail.IdReplacement,
                    orderDetail.Quantity
                );
                return Ok("Order detail added successfully.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(new ApiResponse(404, ex.Message));
                }
                if (ex.Message.Contains("Insufficient stock"))
                {
                    return BadRequest(new ApiResponse(400, ex.Message));
                }
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}