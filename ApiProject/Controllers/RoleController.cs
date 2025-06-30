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
    [Authorize(Roles = "Administrator")]
    public class RoleController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RoleDto>>> Get()
        {
            var roles = await _unitOfWork.Role.GetAllAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleDto>> Get(int id)
        {
            var role = await _unitOfWork.Role.GetByIdAsync(id);
            if (role == null)
                return NotFound($"Role with id {id} was not found.");
            return _mapper.Map<RoleDto>(role);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Role>> Post(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            _unitOfWork.Role.Add(role);
            await _unitOfWork.SaveAsync();
            if (role == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] RoleDto roleDto)
        {
            if (roleDto == null)
                return NotFound();

            var role = _mapper.Map<Role>(roleDto);
            _unitOfWork.Role.Update(role);
            await _unitOfWork.SaveAsync();
            return Ok(role);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _unitOfWork.Role.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            _unitOfWork.Role.Remove(role);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}