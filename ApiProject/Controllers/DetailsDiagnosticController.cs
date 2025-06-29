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
    public class DetailsDiagnosticController : ControllerBase
    {
        private readonly IDetailsDiagnosticRepository _repository;
        public DetailsDiagnosticController(IDetailsDiagnosticRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

        [HttpGet("{diagnosticId}/{serviceOrderId}")]
        public async Task<IActionResult> GetByIds(int diagnosticId, int serviceOrderId)
        {
            var entity = await _repository.GetByIdsAsync(diagnosticId, serviceOrderId);
            if (entity == null) return NotFound();
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Create([FromBody] DetailsDiagnostic entity)
        {
            // No hay Add en la interfaz, se debe implementar si es necesario
            return BadRequest("Método no implementado en el repositorio");
        }

        [HttpPut]
        public IActionResult Update([FromBody] DetailsDiagnostic entity)
        {
            _repository.Update(entity);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] DetailsDiagnostic entity)
        {
            _repository.Remove(entity);
            return NoContent();
        }
    }
} 