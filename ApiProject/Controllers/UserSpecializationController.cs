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
    [Authorize(Roles = "Administrator")]
    public class UserSpecializationController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserSpecializationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserSpecializationDto>>> Get()
        {
            var userSpecializations = await _unitOfWork.UserSpecialization.GetAllAsync();
            return _mapper.Map<List<UserSpecializationDto>>(userSpecializations);
        }

        [HttpGet("{idUser:int}/{idSpecialization:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserSpecializationDto>> Get(int idUser, int idSpecialization)
        {
            var userSpecialization = await _unitOfWork.UserSpecialization.GetByIdsAsync(idUser, idSpecialization);
            if (userSpecialization == null)
                return NotFound($"UserSpecialization with idUser {idUser} and idSpecialization {idSpecialization} was not found.");
            return _mapper.Map<UserSpecializationDto>(userSpecialization);
        }

        [HttpPut("{idUser:int}/{idSpecialization:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int idUser, int idSpecialization, [FromBody] UserSpecializationDto userSpecializationDto)
        {
            if (userSpecializationDto == null)
                return NotFound();

            var userSpecialization = _mapper.Map<UserSpecialization>(userSpecializationDto);
            _unitOfWork.UserSpecialization.Update(userSpecialization);
            await _unitOfWork.SaveAsync();
            return Ok(userSpecialization);
        }

        [HttpDelete("{idUser:int}/{idSpecialization:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int idUser, int idSpecialization)
        {
            var userSpecialization = await _unitOfWork.UserSpecialization.GetByIdsAsync(idUser, idSpecialization);
            if (userSpecialization == null)
                return NotFound();
            _unitOfWork.UserSpecialization.Remove(userSpecialization);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}