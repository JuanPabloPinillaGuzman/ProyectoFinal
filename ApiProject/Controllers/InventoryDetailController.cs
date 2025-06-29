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
    public class InventoryDetailController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryDetailController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<InventoryDetailDto>>> Get()
        {
            var inventoryDetails = await _unitOfWork.InventoryDetail.GetAllAsync();
            return _mapper.Map<List<InventoryDetailDto>>(inventoryDetails);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InventoryDetailDto>> Get(int id)
        {
            var inventoryDetail = await _unitOfWork.InventoryDetail.GetByIdAsync(id);
            if (inventoryDetail == null)
                return NotFound($"InventoryDetail with id {id} was not found.");
            return _mapper.Map<InventoryDetailDto>(inventoryDetail);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InventoryDetail>> Post(InventoryDetailDto inventoryDetailDto)
        {
            var inventoryDetail = _mapper.Map<InventoryDetail>(inventoryDetailDto);
            _unitOfWork.InventoryDetail.Add(inventoryDetail);
            await _unitOfWork.SaveAsync();
            if (inventoryDetail == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = inventoryDetail.Id }, inventoryDetail);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] InventoryDetailDto inventoryDetailDto)
        {
            if (inventoryDetailDto == null)
                return NotFound();

            var inventoryDetail = _mapper.Map<InventoryDetail>(inventoryDetailDto);
            _unitOfWork.InventoryDetail.Update(inventoryDetail);
            await _unitOfWork.SaveAsync();
            return Ok(inventoryDetail);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var inventoryDetail = await _unitOfWork.InventoryDetail.GetByIdAsync(id);
            if (inventoryDetail == null)
                return NotFound();
            _unitOfWork.InventoryDetail.Remove(inventoryDetail);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}