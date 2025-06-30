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
    public class RefreshTokenController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RefreshTokenController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RefreshTokenDto>>> Get()
        {
            var RefreshToken = await _unitOfWork.RefreshToken.GetAllAsync();
            return _mapper.Map<List<RefreshTokenDto>>(RefreshToken);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefreshTokenDto>> Get(int id)
        {
            var RefreshToken = await _unitOfWork.RefreshToken.GetByIdAsync(id);
            if (RefreshToken == null)
            {
                return NotFound($"Refresh Token with id {id} was not found.");
            }
            return _mapper.Map<RefreshTokenDto>(RefreshToken);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RefreshToken>> Post(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = _mapper.Map<RefreshToken>(refreshTokenDto);
            _unitOfWork.RefreshToken.Add(refreshToken);
            await _unitOfWork.SaveAsync();
            if (refreshTokenDto == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = refreshTokenDto.Id }, refreshTokenDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] RefreshTokenDto refreshTokenDto)
        {
            // Validación: objeto nulo
            if (refreshTokenDto == null)
                return NotFound();
            var refreshToken = _mapper.Map<RefreshToken>(refreshTokenDto);
            _unitOfWork.RefreshToken.Update(refreshToken);
            await _unitOfWork.SaveAsync();
            return Ok(refreshTokenDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var RefreshToken = await _unitOfWork.RefreshToken.GetByIdAsync(id);
            if (RefreshToken == null)
                return NotFound();
            _unitOfWork.RefreshToken.Remove(RefreshToken);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}