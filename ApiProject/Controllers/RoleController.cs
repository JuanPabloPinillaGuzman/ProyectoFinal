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
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> Get()
        {
            var roles = await _roleRepository.GetAllAsync();
            var roleDtos = new List<RoleDto>();
            foreach (var r in roles)
            {
                roleDtos.Add(new RoleDto
                {
                    Id = r.Id,

                });
            }
            return Ok(roleDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _roleRepository.GetAllAsync(pageNumber, pageSize, search);
            var roleDtos = new List<RoleDto>();
            foreach (var r in registers)
            {
                roleDtos.Add(new RoleDto
                {
                    Id = r.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(roleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> Get(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound($"Role with id {id} was not found.");
            var dto = new RoleDto
            {
                Id = role.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Role> Post(RoleDto roleDto)
        {
            if (roleDto == null)
                return BadRequest();
            var role = new Role
            {
                Id = roleDto.Id,

            };
            _roleRepository.Add(role);
            return CreatedAtAction(nameof(Post), new { id = roleDto.Id }, role);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RoleDto roleDto)
        {
            if (roleDto == null)
                return NotFound();
            var role = new Role
            {
                Id = roleDto.Id,

            };
            _roleRepository.Update(role);
            return Ok(roleDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            _roleRepository.Remove(role);
            return NoContent();
        }
    }
}