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
    public class StateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StateController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateDto>>> Get()
        {
            var states = await _stateRepository.GetAllAsync();
            var stateDtos = new List<StateDto>();
            foreach (var s in states)
            {
                stateDtos.Add(new StateDto
                {
                    Id = s.Id,

                });
            }
            return Ok(stateDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<StateDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _stateRepository.GetAllAsync(pageNumber, pageSize, search);
            var stateDtos = new List<StateDto>();
            foreach (var s in registers)
            {
                stateDtos.Add(new StateDto
                {
                    Id = s.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(stateDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StateDto>> Get(int id)
        {
            var state = await _stateRepository.GetByIdAsync(id);
            if (state == null)
                return NotFound($"State with id {id} was not found.");
            var dto = new StateDto
            {
                Id = state.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<State> Post(StateDto stateDto)
        {
            if (stateDto == null)
                return BadRequest();
            var state = new State
            {
                Id = stateDto.Id,

            };
            _stateRepository.Add(state);
            return CreatedAtAction(nameof(Post), new { id = stateDto.Id }, state);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] StateDto stateDto)
        {
            if (stateDto == null)
                return NotFound();
            var state = new State
            {
                Id = stateDto.Id,

            };
            _stateRepository.Update(state);
            return Ok(stateDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var state = await _stateRepository.GetByIdAsync(id);
            if (state == null)
                return NotFound();
            _stateRepository.Remove(state);
            return NoContent();
        }
    }
}