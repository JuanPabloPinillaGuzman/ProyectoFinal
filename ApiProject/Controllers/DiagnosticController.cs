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
    public class DiagnosticController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiagnosticController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosticDto>>> Get()
        {
            var diagnostics = await _diagnosticRepository.GetAllAsync();
            var diagnosticDtos = new List<DiagnosticDto>();
            foreach (var d in diagnostics)
            {
                diagnosticDtos.Add(new DiagnosticDto
                {
                    Id = d.Id,

                });
            }
            return Ok(diagnosticDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<DiagnosticDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _diagnosticRepository.GetAllAsync(pageNumber, pageSize, search);
            var diagnosticDtos = new List<DiagnosticDto>();
            foreach (var d in registers)
            {
                diagnosticDtos.Add(new DiagnosticDto
                {
                    Id = d.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(diagnosticDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiagnosticDto>> Get(int id)
        {
            var diagnostic = await _diagnosticRepository.GetByIdAsync(id);
            if (diagnostic == null)
                return NotFound($"Diagnostic with id {id} was not found.");
            var dto = new DiagnosticDto
            {
                Id = diagnostic.Id,
 
            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Diagnostic> Post(DiagnosticDto diagnosticDto)
        {
            if (diagnosticDto == null)
                return BadRequest();
            var diagnostic = new Diagnostic
            {
                Id = diagnosticDto.Id,

            };
            _diagnosticRepository.Add(diagnostic);
            return CreatedAtAction(nameof(Post), new { id = diagnosticDto.Id }, diagnostic);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DiagnosticDto diagnosticDto)
        {
            if (diagnosticDto == null)
                return NotFound();
            var diagnostic = new Diagnostic
            {
                Id = diagnosticDto.Id,

            };
            _diagnosticRepository.Update(diagnostic);
            return Ok(diagnosticDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var diagnostic = await _diagnosticRepository.GetByIdAsync(id);
            if (diagnostic == null)
                return NotFound();
            _diagnosticRepository.Remove(diagnostic);
            return NoContent();
        }
    }
}