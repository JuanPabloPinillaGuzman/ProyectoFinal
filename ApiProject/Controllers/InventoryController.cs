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
    public class InventoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> Get()
        {
            var inventories = await _inventoryRepository.GetAllAsync();
            var inventoryDtos = new List<InventoryDto>();
            foreach (var i in inventories)
            {
                inventoryDtos.Add(new InventoryDto
                {
                    Id = i.Id,

                });
            }
            return Ok(inventoryDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _inventoryRepository.GetAllAsync(pageNumber, pageSize, search);
            var inventoryDtos = new List<InventoryDto>();
            foreach (var i in registers)
            {
                inventoryDtos.Add(new InventoryDto
                {
                    Id = i.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(inventoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryDto>> Get(int id)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            if (inventory == null)
                return NotFound($"Inventory with id {id} was not found.");
            var dto = new InventoryDto
            {
                Id = inventory.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Inventory> Post(InventoryDto inventoryDto)
        {
            if (inventoryDto == null)
                return BadRequest();
            var inventory = new Inventory
            {
                Id = inventoryDto.Id,

            };
            _inventoryRepository.Add(inventory);
            return CreatedAtAction(nameof(Post), new { id = inventoryDto.Id }, inventory);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] InventoryDto inventoryDto)
        {
            if (inventoryDto == null)
                return NotFound();
            var inventory = new Inventory
            {
                Id = inventoryDto.Id,

            };
            _inventoryRepository.Update(inventory);
            return Ok(inventoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(id);
            if (inventory == null)
                return NotFound();
            _inventoryRepository.Remove(inventory);
            return NoContent();
        }
    }
}