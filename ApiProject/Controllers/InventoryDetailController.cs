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
    public class InventoryDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventoryDetailController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDetailDto>>> Get()
        {
            var inventoryDetails = await _inventoryDetailRepository.GetAllAsync();
            var inventoryDetailDtos = new List<InventoryDetailDto>();
            foreach (var i in inventoryDetails)
            {
                inventoryDetailDtos.Add(new InventoryDetailDto
                {
                    Id = i.Id,

                });
            }
            return Ok(inventoryDetailDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<InventoryDetailDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _inventoryDetailRepository.GetAllAsync(pageNumber, pageSize, search);
            var inventoryDetailDtos = new List<InventoryDetailDto>();
            foreach (var i in registers)
            {
                inventoryDetailDtos.Add(new InventoryDetailDto
                {
                    Id = i.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(inventoryDetailDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryDetailDto>> Get(int id)
        {
            var inventoryDetail = await _inventoryDetailRepository.GetByIdAsync(id);
            if (inventoryDetail == null)
                return NotFound($"InventoryDetail with id {id} was not found.");
            var dto = new InventoryDetailDto
            {
                Id = inventoryDetail.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<InventoryDetail> Post(InventoryDetailDto inventoryDetailDto)
        {
            if (inventoryDetailDto == null)
                return BadRequest();
            var inventoryDetail = new InventoryDetail
            {
                Id = inventoryDetailDto.Id,

            };
            _inventoryDetailRepository.Add(inventoryDetail);
            return CreatedAtAction(nameof(Post), new { id = inventoryDetailDto.Id }, inventoryDetail);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] InventoryDetailDto inventoryDetailDto)
        {
            if (inventoryDetailDto == null)
                return NotFound();
            var inventoryDetail = new InventoryDetail
            {
                Id = inventoryDetailDto.Id,

            };
            _inventoryDetailRepository.Update(inventoryDetail);
            return Ok(inventoryDetailDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inventoryDetail = await _inventoryDetailRepository.GetByIdAsync(id);
            if (inventoryDetail == null)
                return NotFound();
            _inventoryDetailRepository.Remove(inventoryDetail);
            return NoContent();
        }
    }
}