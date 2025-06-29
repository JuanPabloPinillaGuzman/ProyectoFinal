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
    public class InventoryController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> Get()
        {
            var inventories = await _unitOfWork.Inventory.GetAllAsync();
            return _mapper.Map<List<InventoryDto>>(inventories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InventoryDto>> Get(int id)
        {
            var inventory = await _unitOfWork.Inventory.GetByIdAsync(id);
            if (inventory == null)
                return NotFound($"Inventory with id {id} was not found.");
            return _mapper.Map<InventoryDto>(inventory);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Inventory>> Post(InventoryDto inventoryDto)
        {
            var inventory = _mapper.Map<Inventory>(inventoryDto);
            _unitOfWork.Inventory.Add(inventory);
            await _unitOfWork.SaveAsync();
            if (inventory == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = inventory.Id }, inventory);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] InventoryDto inventoryDto)
        {
            if (inventoryDto == null)
                return NotFound();

            var inventory = _mapper.Map<Inventory>(inventoryDto);
            _unitOfWork.Inventory.Update(inventory);
            await _unitOfWork.SaveAsync();
            return Ok(inventory);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _unitOfWork.Inventory.GetByIdAsync(id);
            if (inventory == null)
                return NotFound();
            _unitOfWork.Inventory.Remove(inventory);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}