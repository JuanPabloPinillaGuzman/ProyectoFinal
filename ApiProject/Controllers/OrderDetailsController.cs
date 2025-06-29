using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> Get()
        {
            var orderDetails = await _orderDetailsRepository.GetAllAsync();
            var orderDetailDtos = new List<OrderDetailDto>();
            foreach (var o in orderDetails)
            {
                orderDetailDtos.Add(new OrderDetailDto
                {
                    Id = o.Id,

                });
            }
            return Ok(orderDetailDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _orderDetailsRepository.GetAllAsync(pageNumber, pageSize, search);
            var orderDetailDtos = new List<OrderDetailDto>();
            foreach (var o in registers)
            {
                orderDetailDtos.Add(new OrderDetailDto
                {
                    Id = o.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(orderDetailDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDto>> Get(int id)
        {
            var orderDetail = await _orderDetailsRepository.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound($"OrderDetail with id {id} was not found.");
            var dto = new OrderDetailDto
            {
                Id = orderDetail.Id,
            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<OrderDetails> Post(OrderDetailDto orderDetailDto)
        {
            if (orderDetailDto == null)
                return BadRequest();
            var orderDetail = new OrderDetails
            {
                Id = orderDetailDto.Id,

            };
            _orderDetailsRepository.Add(orderDetail);
            return CreatedAtAction(nameof(Post), new { id = orderDetailDto.Id }, orderDetail);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] OrderDetailDto orderDetailDto)
        {
            if (orderDetailDto == null)
                return NotFound();
            var orderDetail = new OrderDetails
            {
                Id = orderDetailDto.Id,

            };
            _orderDetailsRepository.Update(orderDetail);
            return Ok(orderDetailDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var orderDetail = await _orderDetailsRepository.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound();
            _orderDetailsRepository.Remove(orderDetail);
            return NoContent();
        }
    }
}