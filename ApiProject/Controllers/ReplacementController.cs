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
    public class ReplacementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReplacementController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReplacementDto>>> Get()
        {
            var replacements = await _replacementRepository.GetAllAsync();
            var replacementDtos = new List<ReplacementDto>();
            foreach (var r in replacements)
            {
                replacementDtos.Add(new ReplacementDto
                {
                    Id = r.Id,

                });
            }
            return Ok(replacementDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ReplacementDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _replacementRepository.GetAllAsync(pageNumber, pageSize, search);
            var replacementDtos = new List<ReplacementDto>();
            foreach (var r in registers)
            {
                replacementDtos.Add(new ReplacementDto
                {
                    Id = r.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(replacementDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReplacementDto>> Get(int id)
        {
            var replacement = await _replacementRepository.GetByIdAsync(id);
            if (replacement == null)
                return NotFound($"Replacement with id {id} was not found.");
            var dto = new ReplacementDto
            {
                Id = replacement.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Replacement> Post(ReplacementDto replacementDto)
        {
            if (replacementDto == null)
                return BadRequest();
            var replacement = new Replacement
            {
                Id = replacementDto.Id,

            };
            _replacementRepository.Add(replacement);
            return CreatedAtAction(nameof(Post), new { id = replacementDto.Id }, replacement);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ReplacementDto replacementDto)
        {
            if (replacementDto == null)
                return NotFound();
            var replacement = new Replacement
            {
                Id = replacementDto.Id,

            };
            _replacementRepository.Update(replacement);
            return Ok(replacementDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var replacement = await _replacementRepository.GetByIdAsync(id);
            if (replacement == null)
                return NotFound();
            _replacementRepository.Remove(replacement);
            return NoContent();
        }
    }
}