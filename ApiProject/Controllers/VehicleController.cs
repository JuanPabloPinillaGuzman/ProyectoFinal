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
    public class VehicleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Get()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();

            var vehicleDtos = new List<VehicleDto>();
            foreach (var v in vehicles)
            {
                vehicleDtos.Add(new VehicleDto
                {
                    Id = v.Id,

                });
            }
            return Ok(vehicleDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _vehicleRepository.GetAllAsync(pageNumber, pageSize, search);
            var vehicleDtos = new List<VehicleDto>();
            foreach (var v in registers)
            {
                vehicleDtos.Add(new VehicleDto
                {
                    Id = v.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(vehicleDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleDto>> Get(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound($"Vehicle with id {id} was not found.");
            var dto = new VehicleDto
            {
                Id = vehicle.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Vehicle> Post(VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
                return BadRequest();
            var vehicle = new Vehicle
            {
                Id = vehicleDto.Id,

            };
            _vehicleRepository.Add(vehicle);

            return CreatedAtAction(nameof(Post), new { id = vehicleDto.Id }, vehicle);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
                return NotFound();
            var vehicle = new Vehicle
            {
                Id = vehicleDto.Id,

            };
            _vehicleRepository.Update(vehicle);

            return Ok(vehicleDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();
            _vehicleRepository.Remove(vehicle);

            return NoContent();
        }
    }
}