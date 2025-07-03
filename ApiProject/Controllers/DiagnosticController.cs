using System;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiProject.Controllers;
using Application.DTOs;
using AutoMapper;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "administrator, receptionist")]
    public class DiagnosticController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiagnosticController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DiagnosticDto>>> Get()
        {
            var diagnostics = await _unitOfWork.Diagnostic.GetAllAsync();
            return _mapper.Map<List<DiagnosticDto>>(diagnostics);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DiagnosticDto>> Get(int id)
        {
            var diagnostic = await _unitOfWork.Diagnostic.GetByIdAsync(id);
            if (diagnostic == null)
                return NotFound($"Diagnostic with id {id} was not found.");
            return _mapper.Map<DiagnosticDto>(diagnostic);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Diagnostic>> Post(DiagnosticDto diagnosticDto)
        {
            var diagnostic = _mapper.Map<Diagnostic>(diagnosticDto);
            _unitOfWork.Diagnostic.Add(diagnostic);
            await _unitOfWork.SaveAsync();
            if (diagnostic == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = diagnostic.Id }, diagnostic);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] DiagnosticDto diagnosticDto)
        {
            if (diagnosticDto == null)
                return NotFound();

            var diagnostic = _mapper.Map<Diagnostic>(diagnosticDto);
            _unitOfWork.Diagnostic.Update(diagnostic);
            await _unitOfWork.SaveAsync();
            return Ok(diagnostic);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var diagnostic = await _unitOfWork.Diagnostic.GetByIdAsync(id);
            if (diagnostic == null)
                return NotFound();
            _unitOfWork.Diagnostic.Remove(diagnostic);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}