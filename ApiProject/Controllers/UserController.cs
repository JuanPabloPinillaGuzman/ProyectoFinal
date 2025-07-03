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
    [Authorize(Roles = "administrator")]
    public class UserController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var users = await _unitOfWork.User.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with id {id} was not found.");
            return _mapper.Map<UserDto>(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> Post(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            _unitOfWork.User.Add(user);
            await _unitOfWork.SaveAsync();
            if (user == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
                return NotFound();

            var user = _mapper.Map<User>(userDto);
            _unitOfWork.User.Update(user);
            await _unitOfWork.SaveAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            _unitOfWork.User.Remove(user);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}