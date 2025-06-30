using System;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiProject.Controllers;
using Application.DTOs;
using AutoMapper;

namespace ApiProject.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]
    // [Authorize(Roles = "Administrator, Mechanic")]
    public class DetailsDiagnosticController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetailsDiagnosticController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DetailsDiagnosticDto>>> Get()
        {
            var detailsDiagnostics = await _unitOfWork.DetailsDiagnostic.GetAllAsync();
            return _mapper.Map<List<DetailsDiagnosticDto>>(detailsDiagnostics);
        }

        [HttpGet("{idServiceOrder:int}/{idDiagnostic:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetailsDiagnosticDto>> Get(int idServiceOrder, int idDiagnostic)
        {
            var detailsDiagnostic = await _unitOfWork.DetailsDiagnostic.GetByIdsAsync(idServiceOrder, idDiagnostic);
            if (detailsDiagnostic == null)
                return NotFound($"DetailsDiagnostic with idServiceOrder {idServiceOrder} and idDiagnostic {idDiagnostic} was not found.");
            return _mapper.Map<DetailsDiagnosticDto>(detailsDiagnostic);
        }

        [HttpPut("{idServiceOrder:int}/{idDiagnostic:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int idServiceOrder, int idDiagnostic, [FromBody] DetailsDiagnosticDto detailsDiagnosticoDto)
        {
            if (detailsDiagnosticoDto == null)
                return NotFound();

            var detailsDiagnostic = _mapper.Map<DetailsDiagnostic>(detailsDiagnosticoDto);
            _unitOfWork.DetailsDiagnostic.Update(detailsDiagnostic);
            await _unitOfWork.SaveAsync();
            return Ok(detailsDiagnostic);
        }

        [HttpDelete("{idServiceOrder:int}/{idDiagnostic:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int idServiceOrder, int idDiagnostic)
        {
            var detailsDiagnostic = await _unitOfWork.DetailsDiagnostic.GetByIdsAsync(idServiceOrder, idDiagnostic);
            if (detailsDiagnostic == null)
                return NotFound();
            _unitOfWork.DetailsDiagnostic.Remove(detailsDiagnostic);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}