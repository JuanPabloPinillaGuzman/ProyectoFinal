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
    public class ClientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> Get()
        {
            var clients = await _clientRepository.GetAllAsync();
            var clientDtos = new List<ClientDto>();
            foreach (var c in clients)
            {
                clientDtos.Add(new ClientDto
                {
                    Id = c.Id,

                });
            }
            return Ok(clientDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _clientRepository.GetAllAsync(pageNumber, pageSize, search);
            var clientDtos = new List<ClientDto>();
            foreach (var c in registers)
            {
                clientDtos.Add(new ClientDto
                {
                    Id = c.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> Get(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return NotFound($"Client with id {id} was not found.");
            var dto = new ClientDto
            {
                Id = client.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Client> Post(ClientDto clientDto)
        {
            if (clientDto == null)
                return BadRequest();
            var client = new Client
            {
                Id = clientDto.Id,

            };
            _clientRepository.Add(client);
            return CreatedAtAction(nameof(Post), new { id = clientDto.Id }, client);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ClientDto clientDto)
        {
            if (clientDto == null)
                return NotFound();
            var client = new Client
            {
                Id = clientDto.Id,

            };
            _clientRepository.Update(client);
            return Ok(clientDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return NotFound();
            _clientRepository.Remove(client);
            return NoContent();
        }
    }
}