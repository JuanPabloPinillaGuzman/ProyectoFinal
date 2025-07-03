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
    public class UserRoleController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserRoleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserRoleDto>>> Get()
        {
            var userRoles = await _unitOfWork.UserRole.GetAllAsync();
            return _mapper.Map<List<UserRoleDto>>(userRoles);
        }

        [HttpGet("{idUser:int}/{idRole:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserRoleDto>> Get(int idUser, int idRole)
        {
            var userRole = await _unitOfWork.UserRole.GetByIdsAsync(idUser, idRole);
            if (userRole == null)
                return NotFound($"UserRole with idUser {idUser} and idRole {idRole} was not found.");
            return _mapper.Map<UserRoleDto>(userRole);
        }

        [HttpPut("{idUser:int}/{idRole:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int idUser, int idRole, [FromBody] UserRoleDto userRoleDto)
        {
            if (userRoleDto == null)
                return NotFound();

            var userRole = _mapper.Map<UserRole>(userRoleDto);
            _unitOfWork.UserRole.Update(userRole);
            await _unitOfWork.SaveAsync();
            return Ok(userRole);
        }

        [HttpDelete("{idUser:int}/{idRole:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int idUser, int idRole)
        {
            var userRole = await _unitOfWork.UserRole.GetByIdsAsync(idUser, idRole);
            if (userRole == null)
                return NotFound();
            _unitOfWork.UserRole.Remove(userRole);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}