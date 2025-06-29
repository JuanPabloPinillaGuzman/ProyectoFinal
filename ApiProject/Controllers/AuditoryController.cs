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
    public class AuditoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditoryDto>>> Get()
        {
            var auditories = await _auditoryRepository.GetAllAsync();
            var auditoryDtos = new List<AuditoryDto>();
            foreach (var a in auditories)
            {
                auditoryDtos.Add(new AuditoryDto
                {
                    Id = a.Id,

                });
            }
            return Ok(auditoryDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<AuditoryDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _auditoryRepository.GetAllAsync(pageNumber, pageSize, search);
            var auditoryDtos = new List<AuditoryDto>();
            foreach (var a in registers)
            {
                auditoryDtos.Add(new AuditoryDto
                {
                    Id = a.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(auditoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuditoryDto>> Get(int id)
        {
            var auditory = await _auditoryRepository.GetByIdAsync(id);
            if (auditory == null)
                return NotFound($"Auditory with id {id} was not found.");
            var dto = new AuditoryDto
            {
                Id = auditory.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Auditory> Post(AuditoryDto auditoryDto)
        {
            if (auditoryDto == null)
                return BadRequest();
            var auditory = new Auditory
            {
                Id = auditoryDto.Id,

            };
            _auditoryRepository.Add(auditory);
            return CreatedAtAction(nameof(Post), new { id = auditoryDto.Id }, auditory);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AuditoryDto auditoryDto)
        {
            if (auditoryDto == null)
                return NotFound();
            var auditory = new Auditory
            {
                Id = auditoryDto.Id,

            };
            _auditoryRepository.Update(auditory);
            return Ok(auditoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var auditory = await _auditoryRepository.GetByIdAsync(id);
            if (auditory == null)
                return NotFound();
            _auditoryRepository.Remove(auditory);
            return NoContent();
        }
    }
}