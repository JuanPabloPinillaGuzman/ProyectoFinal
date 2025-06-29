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
    public class ServiceTypeController : ControllerBase
    {
        private readonly IServiceTypeRepository _repository;
        public ServiceTypeController(IServiceTypeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ServiceType entity)
        {
            _repository.Add(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ServiceType entity)
        {
            if (id != entity.Id) return BadRequest();
            _repository.Update(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _repository.GetByIdAsync(id).Result;
            if (entity == null) return NotFound();
            _repository.Remove(entity);
            return NoContent();
        }
    }
} 