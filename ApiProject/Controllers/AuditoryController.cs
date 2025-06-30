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
    public class AuditoryController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuditoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AuditoryDto>>> Get()
        {
            var auditories = await _unitOfWork.Auditory.GetAllAsync();
            return _mapper.Map<List<AuditoryDto>>(auditories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuditoryDto>> Get(int id)
        {
            var auditory = await _unitOfWork.Auditory.GetByIdAsync(id);
            if (auditory == null)
                return NotFound($"Auditory with id {id} was not found.");
            return _mapper.Map<AuditoryDto>(auditory);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Auditory>> Post(AuditoryDto auditoryDto)
        {
            var auditory = _mapper.Map<Auditory>(auditoryDto);
            _unitOfWork.Auditory.Add(auditory);
            await _unitOfWork.SaveAsync();
            if (auditory == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = auditory.Id }, auditory);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] AuditoryDto auditoryDto)
        {
            if (auditoryDto == null)
                return NotFound();

            var auditory = _mapper.Map<Auditory>(auditoryDto);
            _unitOfWork.Auditory.Update(auditory);
            await _unitOfWork.SaveAsync();
            return Ok(auditory);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var auditory = await _unitOfWork.Auditory.GetByIdAsync(id);
            if (auditory == null)
                return NotFound();
            _unitOfWork.Auditory.Remove(auditory);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}