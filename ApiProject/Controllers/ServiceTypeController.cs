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
        public async Task<ActionResult<IEnumerable<ServiceTypeDto>>> Get()
        {
            var serviceTypes = await _serviceTypeRepository.GetAllAsync();
            var serviceTypeDtos = new List<ServiceTypeDto>();
            foreach (var s in serviceTypes)
            {
                serviceTypeDtos.Add(new ServiceTypeDto
                {
                    Id = s.Id,

                });
            }
            return Ok(serviceTypeDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ServiceTypeDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _serviceTypeRepository.GetAllAsync(pageNumber, pageSize, search);
            var serviceTypeDtos = new List<ServiceTypeDto>();
            foreach (var s in registers)
            {
                serviceTypeDtos.Add(new ServiceTypeDto
                {
                    Id = s.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(serviceTypeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceTypeDto>> Get(int id)
        {
            var serviceType = await _serviceTypeRepository.GetByIdAsync(id);
            if (serviceType == null)
                return NotFound($"ServiceType with id {id} was not found.");
            var dto = new ServiceTypeDto
            {
                Id = serviceType.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<ServiceType> Post(ServiceTypeDto serviceTypeDto)
        {
            if (serviceTypeDto == null)
                return BadRequest();
            var serviceType = new ServiceType
            {
                Id = serviceTypeDto.Id,

            };
            _serviceTypeRepository.Add(serviceType);
            return CreatedAtAction(nameof(Post), new { id = serviceTypeDto.Id }, serviceType);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ServiceTypeDto serviceTypeDto)
        {
            if (serviceTypeDto == null)
                return NotFound();
            var serviceType = new ServiceType
            {
                Id = serviceTypeDto.Id,

            };
            _serviceTypeRepository.Update(serviceType);
            return Ok(serviceTypeDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceType = await _serviceTypeRepository.GetByIdAsync(id);
            if (serviceType == null)
                return NotFound();
            _serviceTypeRepository.Remove(serviceType);
            return NoContent();
        }
    }
}