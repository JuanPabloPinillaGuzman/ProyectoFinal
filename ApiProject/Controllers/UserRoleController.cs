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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleController(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRoleDto>>> Get()
        {
            var userRoles = await _userRoleRepository.GetAllAsync();
            var userRoleDtos = new List<UserRoleDto>();
            foreach (var ur in userRoles)
            {
                userRoleDtos.Add(new UserRoleDto
                {
                    UserId = ur.UserId,
                    RoleId = ur.RoleId,

                });
            }
            return Ok(userRoleDtos);
        }

        [HttpGet("{userId}/{roleId}")]
        public async Task<ActionResult<UserRoleDto>> GetByIds(int userId, int roleId)
        {
            var userRole = await _userRoleRepository.GetByIdsAsync(userId, roleId);
            if (userRole == null)
                return NotFound($"UserRole with userId {userId} and roleId {roleId} was not found.");
            var dto = new UserRoleDto
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId,

            };
            return Ok(dto);
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null)
                return NotFound();
            var userRole = new UserRole
            {
                UserId = userRoleDto.UserId,
                RoleId = userRoleDto.RoleId,

            };
            _userRoleRepository.Update(userRole);
            return Ok(userRoleDto);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null)
                return NotFound();
            var userRole = new UserRole
            {
                UserId = userRoleDto.UserId,
                RoleId = userRoleDto.RoleId,

            };
            _userRoleRepository.Remove(userRole);
            return NoContent();
        }
    }
}