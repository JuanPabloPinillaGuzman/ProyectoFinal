using System;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using ApiProject.Controllers;
using Application.DTOs;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleRepository _repository;
        public UserRoleController(IUserRoleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{userId}/{rolId}")]
        public async Task<IActionResult> GetByIds(int userId, int rolId)
        {
            var entity = await _repository.GetByIdsAsync(userId, rolId);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UserRole entity)
        {
            _repository.Update(entity);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] UserRole entity)
        {
            _repository.Remove(entity);
            return NoContent();
        }
    }
} 