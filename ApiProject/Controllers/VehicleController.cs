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
using Microsoft.AspNetCore.Authorization;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator, Recepcionist")]
    public class VehicleController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> Get()
        {
            var vehicles = await _unitOfWork.Vehicle.GetAllAsync();
            return _mapper.Map<List<VehicleDto>>(vehicles);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VehicleDto>> Get(int id)
        {
            var vehicle = await _unitOfWork.Vehicle.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound($"Vehicle with id {id} was not found.");
            return _mapper.Map<VehicleDto>(vehicle);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Vehicle>> Post(VehicleDto vehicleDto)
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            _unitOfWork.Vehicle.Add(vehicle);
            await _unitOfWork.SaveAsync();
            if (vehicle == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = vehicle.Id }, vehicle);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
                return NotFound();

            var ordenesActivas = await _unitOfWork.ServiceOrder.GetOrdersByVehicleAsync(id);

            if (ordenesActivas)
            {
                return Conflict("Cannot update vehicle with active service orders.");
            }

            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            _unitOfWork.Vehicle.Update(vehicle);
            await _unitOfWork.SaveAsync();
            return Ok(vehicle);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _unitOfWork.Vehicle.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            var ordenesActivas = await _unitOfWork.ServiceOrder.GetOrdersByVehicleAsync(id);

            if (ordenesActivas)
            {
                return Conflict("Cannot delete vehicle with active service orders.");
            }
            _unitOfWork.Vehicle.Remove(vehicle);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet("pages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (allRegisters, registers) = await _unitOfWork.Vehicle.GetAllAsync(pageNumber, pageSize, search);
            var vehicleDtos = _mapper.Map<List<VehicleDto>>(registers);

            // Agregar X-Total-Count en los encabezados HTTP
            Response.Headers.Append("X-Total-Count", allRegisters.ToString());

            return Ok(vehicleDtos);
        }
    }
}