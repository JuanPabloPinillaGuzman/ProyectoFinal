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
using AutoMapper;

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
    }
}