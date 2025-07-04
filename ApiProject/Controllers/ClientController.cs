using System;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ApiProject.Controllers;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Application.Services;
using ApiProject.Helpers;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "administrator, receptionist, mechanic")]
    public class ClientController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClientDto>>> Get()
        {
            var clients = await _unitOfWork.Client.GetAllAsync();
            return _mapper.Map<List<ClientDto>>(clients);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientDto>> Get(int id)
        {
            var client = await _unitOfWork.Client.GetByIdAsync(id);
            if (client == null)
                return NotFound($"Client with id {id} was not found.");
            return _mapper.Map<ClientDto>(client);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Client>> Post(ClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            _unitOfWork.Client.Add(client);
            await _unitOfWork.SaveAsync();
            if (client == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] ClientDto clientDto)
        {
            if (clientDto == null)
                return NotFound();

            var client = _mapper.Map<Client>(clientDto);
            _unitOfWork.Client.Update(client);
            await _unitOfWork.SaveAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _unitOfWork.Client.GetByIdAsync(id);
            if (client == null)
                return NotFound();
            _unitOfWork.Client.Remove(client);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}