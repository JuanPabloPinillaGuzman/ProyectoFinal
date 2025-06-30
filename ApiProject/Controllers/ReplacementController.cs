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
using Microsoft.AspNetCore.Authorization;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class ReplacementController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReplacementController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReplacementDto>>> Get()
        {
            var replacements = await _unitOfWork.Replacement.GetAllAsync();
            return _mapper.Map<List<ReplacementDto>>(replacements);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReplacementDto>> Get(int id)
        {
            var replacement = await _unitOfWork.Replacement.GetByIdAsync(id);
            if (replacement == null)
                return NotFound($"Replacement with id {id} was not found.");
            return _mapper.Map<ReplacementDto>(replacement);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Replacement>> Post(ReplacementDto replacementDto)
        {
            var replacement = _mapper.Map<Replacement>(replacementDto);
            _unitOfWork.Replacement.Add(replacement);
            await _unitOfWork.SaveAsync();
            if (replacement == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = replacement.Id }, replacement);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] ReplacementDto replacementDto)
        {
            if (replacementDto == null)
                return NotFound();

            var replacement = _mapper.Map<Replacement>(replacementDto);
            _unitOfWork.Replacement.Update(replacement);
            await _unitOfWork.SaveAsync();
            return Ok(replacement);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var replacement = await _unitOfWork.Replacement.GetByIdAsync(id);
            if (replacement == null)
                return NotFound();
            _unitOfWork.Replacement.Remove(replacement);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReplacementDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (allRegisters, registers) = await _unitOfWork.Replacement.GetAllAsync(pageNumber, pageSize, search);
            var replacementDtos = _mapper.Map<List<ReplacementDto>>(registers);

            // Agregar X-Total-Count en los encabezados HTTP
            Response.Headers.Append("X-Total-Count", allRegisters.ToString());

            return Ok(replacementDtos);
        }
    }
}