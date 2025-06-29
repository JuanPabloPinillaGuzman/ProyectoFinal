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
    public class ServiceOrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceOrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceOrderDto>>> Get()
        {
            var serviceOrders = await _serviceOrderRepository.GetAllAsync();
            var serviceOrderDtos = new List<ServiceOrderDto>();
            foreach (var s in serviceOrders)
            {
                serviceOrderDtos.Add(new ServiceOrderDto
                {
                    Id = s.Id,

                });
            }
            return Ok(serviceOrderDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ServiceOrderDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _serviceOrderRepository.GetAllAsync(pageNumber, pageSize, search);
            var serviceOrderDtos = new List<ServiceOrderDto>();
            foreach (var s in registers)
            {
                serviceOrderDtos.Add(new ServiceOrderDto
                {
                    Id = s.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(serviceOrderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceOrderDto>> Get(int id)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
            if (serviceOrder == null)
                return NotFound($"ServiceOrder with id {id} was not found.");
            var dto = new ServiceOrderDto
            {
                Id = serviceOrder.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<ServiceOrder> Post(ServiceOrderDto serviceOrderDto)
        {
            if (serviceOrderDto == null)
                return BadRequest();
            var serviceOrder = new ServiceOrder
            {
                Id = serviceOrderDto.Id,

            };
            _serviceOrderRepository.Add(serviceOrder);
            return CreatedAtAction(nameof(Post), new { id = serviceOrderDto.Id }, serviceOrder);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ServiceOrderDto serviceOrderDto)
        {
            if (serviceOrderDto == null)
                return NotFound();
            var serviceOrder = new ServiceOrder
            {
                Id = serviceOrderDto.Id,

            };
            _serviceOrderRepository.Update(serviceOrder);
            return Ok(serviceOrderDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(id);
            if (serviceOrder == null)
                return NotFound();
            _serviceOrderRepository.Remove(serviceOrder);
            return NoContent();
        }
    }
}