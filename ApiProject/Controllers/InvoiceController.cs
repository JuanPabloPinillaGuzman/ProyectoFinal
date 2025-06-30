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
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Application.Services;
using ApiProject.Helpers.Errors;

namespace ApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator, Mechanic")]
    public class InvoiceController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GenerateInvoiceService _generateInvoice;

        public InvoiceController(IUnitOfWork unitOfWork, IMapper mapper, GenerateInvoiceService generateInvoice)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _generateInvoice = generateInvoice;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> Get()
        {
            var invoices = await _unitOfWork.Invoice.GetAllAsync();
            return _mapper.Map<List<InvoiceDto>>(invoices);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<InvoiceDto>> Get(int id)
        {
            var invoice = await _unitOfWork.Invoice.GetByIdAsync(id);
            if (invoice == null)
                return NotFound($"Invoice with id {id} was not found.");
            return _mapper.Map<InvoiceDto>(invoice);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Invoice>> Post(InvoiceDto invoiceDto)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDto);
            _unitOfWork.Invoice.Add(invoice);
            await _unitOfWork.SaveAsync();
            if (invoice == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Post), new { id = invoice.Id }, invoice);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto == null)
                return NotFound();

            var invoice = _mapper.Map<Invoice>(invoiceDto);
            _unitOfWork.Invoice.Update(invoice);
            await _unitOfWork.SaveAsync();
            return Ok(invoice);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _unitOfWork.Invoice.GetByIdAsync(id);
            if (invoice == null)
                return NotFound();
            _unitOfWork.Invoice.Remove(invoice);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpPost("generate/{serviceOrderId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Generate(int serviceOrderId)
        {
            try
            {
                var invoiceDto = await _generateInvoice.GenerateInvoiceAsync(serviceOrderId);
                return CreatedAtAction(nameof(Get), new { id = invoiceDto.Id }, invoiceDto);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new ApiResponse(404, ex.Message));
                if (ex.Message.Contains("No order details"))
                    return BadRequest(new ApiResponse(400, ex.Message));
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}