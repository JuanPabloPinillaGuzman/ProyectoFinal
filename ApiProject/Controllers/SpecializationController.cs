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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<SpecializationDto>>> Get()
        {
            var specializations = await _unitOfWork.Specialization.GetAllAsync();
            return _mapper.Map<List<SpecializationDto>>(specializations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SpecializationDto>> Get(int id)
        {
            var specialization = await _unitOfWork.Specialization.GetByIdAsync(id);
            if (specialization == null)
                return NotFound($"Specialization with id {id} was not found.");
            return _mapper.Map<SpecializationDto>(specialization);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Specialization>> Post(SpecializationDto specializationDto)
        {
            var specialization = _mapper.Map<Specialization>(specializationDto);
            _unitOfWork.Specialization.Add(specialization);
            await _unitOfWork.SaveAsync();
            if (specialization == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = specialization.Id }, specialization);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] SpecializationDto specializationDto)
        {
            if (specializationDto == null)
                return NotFound();

            var specialization = _mapper.Map<Specialization>(specializationDto);
            _unitOfWork.Specialization.Update(specialization);
            await _unitOfWork.SaveAsync();
            return Ok(specialization);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var specialization = await _unitOfWork.Specialization.GetByIdAsync(id);
            if (specialization == null)
                return NotFound();
            _unitOfWork.Specialization.Remove(specialization);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}