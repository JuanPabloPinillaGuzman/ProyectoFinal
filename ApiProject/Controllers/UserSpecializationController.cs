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
    public class UserSpecializationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserSpecializationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSpecializationDto>>> Get()
        {
            var userSpecializations = await _userSpecializationRepository.GetAllAsync();
            var userSpecializationDtos = new List<UserSpecializationDto>();
            foreach (var us in userSpecializations)
            {
                userSpecializationDtos.Add(new UserSpecializationDto
                {
                    UserId = us.UserId,
                    SpecializationId = us.SpecializationId,

                });
            }
            return Ok(userSpecializationDtos);
        }

        [HttpGet("{userId}/{specializationId}")]
        public async Task<ActionResult<UserSpecializationDto>> GetByIds(int userId, int specializationId)
        {
            var userSpecialization = await _userSpecializationRepository.GetByIdsAsync(userId, specializationId);
            if (userSpecialization == null)
                return NotFound($"UserSpecialization with userId {userId} and specializationId {specializationId} was not found.");
            var dto = new UserSpecializationDto
            {
                UserId = userSpecialization.UserId,
                SpecializationId = userSpecialization.SpecializationId,

            };
            return Ok(dto);
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserSpecializationDto userSpecializationDto)
        {
            if (userSpecializationDto == null)
                return NotFound();
            var userSpecialization = new UserSpecialization
            {
                UserId = userSpecializationDto.UserId,
                SpecializationId = userSpecializationDto.SpecializationId,

            };
            _userSpecializationRepository.Update(userSpecialization);
            return Ok(userSpecializationDto);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] UserSpecializationDto userSpecializationDto)
        {
            if (userSpecializationDto == null)
                return NotFound();
            var userSpecialization = new UserSpecialization
            {
                UserId = userSpecializationDto.UserId,
                SpecializationId = userSpecializationDto.SpecializationId,

            };
            _userSpecializationRepository.Remove(userSpecialization);
            return NoContent();
        }
    }
}