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
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = new List<UserDto>();
            foreach (var u in users)
            {
                userDtos.Add(new UserDto
                {
                    Id = u.Id,

                });
            }
            return Ok(userDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _userRepository.GetAllAsync(pageNumber, pageSize, search);
            var userDtos = new List<UserDto>();
            foreach (var u in registers)
            {
                userDtos.Add(new UserDto
                {
                    Id = u.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User with id {id} was not found.");
            var dto = new UserDto
            {
                Id = user.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<User> Post(UserDto userDto)
        {
            if (userDto == null)
                return BadRequest();
            var user = new User
            {
                Id = userDto.Id,

            };
            _userRepository.Add(user);
            return CreatedAtAction(nameof(Post), new { id = userDto.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDto userDto)
        {
            if (userDto == null)
                return NotFound();
            var user = new User
            {
                Id = userDto.Id,

            };
            _userRepository.Update(user);
            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            _userRepository.Remove(user);
            return NoContent();
        }
    }
}