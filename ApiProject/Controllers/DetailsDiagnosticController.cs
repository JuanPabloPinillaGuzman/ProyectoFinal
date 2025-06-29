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
    public class DetailsDiagnosticController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DetailsDiagnosticController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetailsDiagnosticoDto>>> Get()
        {
            var detailsDiagnostics = await _detailsDiagnosticRepository.GetAllAsync();
            var detailsDiagnosticDtos = new List<DetailsDiagnosticoDto>();
            foreach (var d in detailsDiagnostics)
            {
                detailsDiagnosticDtos.Add(new DetailsDiagnosticoDto
                {
                    DiagnosticId = d.DiagnosticId,
                    ServiceOrderId = d.ServiceOrderId,

                });
            }
            return Ok(detailsDiagnosticDtos);
        }

        [HttpGet("{diagnosticId}/{serviceOrderId}")]
        public async Task<ActionResult<DetailsDiagnosticoDto>> GetByIds(int diagnosticId, int serviceOrderId)
        {
            var detailsDiagnostic = await _detailsDiagnosticRepository.GetByIdsAsync(diagnosticId, serviceOrderId);
            if (detailsDiagnostic == null)
                return NotFound($"DetailsDiagnostic with diagnosticId {diagnosticId} and serviceOrderId {serviceOrderId} was not found.");
            var dto = new DetailsDiagnosticoDto
            {
                DiagnosticId = detailsDiagnostic.DiagnosticId,
                ServiceOrderId = detailsDiagnostic.ServiceOrderId,

            };
            return Ok(dto);
        }

        [HttpPut]
        public IActionResult Put([FromBody] DetailsDiagnosticoDto detailsDiagnosticDto)
        {
            if (detailsDiagnosticDto == null)
                return NotFound();
            var detailsDiagnostic = new DetailsDiagnostic
            {
                DiagnosticId = detailsDiagnosticDto.DiagnosticId,
                ServiceOrderId = detailsDiagnosticDto.ServiceOrderId,

            };
            _detailsDiagnosticRepository.Update(detailsDiagnostic);
            return Ok(detailsDiagnosticDto);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] DetailsDiagnosticoDto detailsDiagnosticDto)
        {
            if (detailsDiagnosticDto == null)
                return NotFound();
            var detailsDiagnostic = new DetailsDiagnostic
            {
                DiagnosticId = detailsDiagnosticDto.DiagnosticId,
                ServiceOrderId = detailsDiagnosticDto.ServiceOrderId,

            };
            _detailsDiagnosticRepository.Remove(detailsDiagnostic);
            return NoContent();
        }
    }
}