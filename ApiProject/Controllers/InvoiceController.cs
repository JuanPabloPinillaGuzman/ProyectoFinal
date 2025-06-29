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
    public class InvoiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InoviceDto>>> Get()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            var invoiceDtos = new List<InoviceDto>();
            foreach (var i in invoices)
            {
                invoiceDtos.Add(new InoviceDto
                {
                    Id = i.Id,

                });
            }
            return Ok(invoiceDtos);
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<InoviceDto>>> GetPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var (totalRegisters, registers) = await _invoiceRepository.GetAllAsync(pageNumber, pageSize, search);
            var invoiceDtos = new List<InoviceDto>();
            foreach (var i in registers)
            {
                invoiceDtos.Add(new InoviceDto
                {
                    Id = i.Id,

                });
            }
            Response.Headers.Add("X-Total-Count", totalRegisters.ToString());
            return Ok(invoiceDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InoviceDto>> Get(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return NotFound($"Invoice with id {id} was not found.");
            var dto = new InoviceDto
            {
                Id = invoice.Id,

            };
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Invoice> Post(InoviceDto invoiceDto)
        {
            if (invoiceDto == null)
                return BadRequest();
            var invoice = new Invoice
            {
                Id = invoiceDto.Id,

            };
            _invoiceRepository.Add(invoice);
            return CreatedAtAction(nameof(Post), new { id = invoiceDto.Id }, invoice);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] InoviceDto invoiceDto)
        {
            if (invoiceDto == null)
                return NotFound();
            var invoice = new Invoice
            {
                Id = invoiceDto.Id,

            };
            _invoiceRepository.Update(invoice);
            return Ok(invoiceDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return NotFound();
            _invoiceRepository.Remove(invoice);
            return NoContent();
        }
    }
}