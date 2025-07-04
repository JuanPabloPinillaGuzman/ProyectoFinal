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
    [Authorize(Roles = "administrator, mechanic")]
    public class OrderDetailsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDetailsDto>>> Get()
        {
            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync();
            return _mapper.Map<List<OrderDetailsDto>>(orderDetails);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetailsDto>> Get(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound($"OrderDetail with id {id} was not found.");
            return _mapper.Map<OrderDetailsDto>(orderDetail);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDetails>> Post(OrderDetailsDto orderDetailDto)
        {
            var orderDetail = _mapper.Map<OrderDetails>(orderDetailDto);
            _unitOfWork.OrderDetails.Add(orderDetail);
            await _unitOfWork.SaveAsync();
            if (orderDetail == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = orderDetail.Id }, orderDetail);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] OrderDetailsDto orderDetailDto)
        {
            if (orderDetailDto == null)
                return NotFound();

            var orderDetail = _mapper.Map<OrderDetails>(orderDetailDto);
            _unitOfWork.OrderDetails.Update(orderDetail);
            await _unitOfWork.SaveAsync();
            return Ok(orderDetail);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound();
            _unitOfWork.OrderDetails.Remove(orderDetail);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}