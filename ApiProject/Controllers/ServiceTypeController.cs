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
    public class ServiceTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ServiceTypeDto>>> Get()
        {
            var serviceTypes = await _unitOfWork.ServiceType.GetAllAsync();
            return _mapper.Map<List<ServiceTypeDto>>(serviceTypes);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceTypeDto>> Get(int id)
        {
            var serviceType = await _unitOfWork.ServiceType.GetByIdAsync(id);
            if (serviceType == null)
                return NotFound($"ServiceType with id {id} was not found.");
            return _mapper.Map<ServiceTypeDto>(serviceType);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceType>> Post(ServiceTypeDto serviceTypeDto)
        {
            var serviceType = _mapper.Map<ServiceType>(serviceTypeDto);
            _unitOfWork.ServiceType.Add(serviceType);
            await _unitOfWork.SaveAsync();
            if (serviceType == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = serviceType.Id }, serviceType);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] ServiceTypeDto serviceTypeDto)
        {
            if (serviceTypeDto == null)
                return NotFound();

            var serviceType = _mapper.Map<ServiceType>(serviceTypeDto);
            _unitOfWork.ServiceType.Update(serviceType);
            await _unitOfWork.SaveAsync();
            return Ok(serviceType);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceType = await _unitOfWork.ServiceType.GetByIdAsync(id);
            if (serviceType == null)
                return NotFound();
            _unitOfWork.ServiceType.Remove(serviceType);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}