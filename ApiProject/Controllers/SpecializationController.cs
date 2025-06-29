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
    public class SpecializationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecializationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> Get()
        {
            var specializations = await _specializationRepository.GetAllAsync();
            var specializationDtos = new List<SpecializationDto>();
            foreach (var s in specializations)
            {
                specializationDtos.Add(new SpecializationDto
                {
                    Id = s.Id,

                });
            }
            return Ok(specializationDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _specializationRepository.GetAllAsync(pageNumber, pageSize, search);
            var specializationDtos = new List<SpecializationDto>();
            foreach (var s in registers)
            {
                specializationDtos.Add(new SpecializationDto
                {
                    Id = s.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(specializationDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpecializationDto>> Get(int id)
        {
            var specialization = await _specializationRepository.GetByIdAsync(id);
            if (specialization == null)
                return NotFound($"Specialization with id {id} was not found.");
            var dto = new SpecializationDto
            {
                Id = specialization.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Specialization> Post(SpecializationDto specializationDto)
        {
            if (specializationDto == null)
                return BadRequest();
            var specialization = new Specialization
            {
                Id = specializationDto.Id,

            };
            _specializationRepository.Add(specialization);
            return CreatedAtAction(nameof(Post), new { id = specializationDto.Id }, specialization);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SpecializationDto specializationDto)
        {
            if (specializationDto == null)
                return NotFound();
            var specialization = new Specialization
            {
                Id = specializationDto.Id,

            };
            _specializationRepository.Update(specialization);
            return Ok(specializationDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var specialization = await _specializationRepository.GetByIdAsync(id);
            if (specialization == null)
                return NotFound();
            _specializationRepository.Remove(specialization);
            return NoContent();
        }
    }
}