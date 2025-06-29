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
    public class UserSpecializationController : ControllerBase
    {
        private readonly IUserSpecializationRepository _repository;
        public UserSpecializationController(IUserSpecializationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{userId}/{specializationId}")]
        public async Task<IActionResult> GetByIds(int userId, int specializationId)
        {
            var entity = await _repository.GetByIdsAsync(userId, specializationId);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UserSpecialization entity)
        {
            _repository.Update(entity);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] UserSpecialization entity)
        {
            _repository.Remove(entity);
            return NoContent();
        }
    }
} 