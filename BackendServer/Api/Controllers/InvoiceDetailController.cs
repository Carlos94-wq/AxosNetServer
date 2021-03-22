using Api.Responses;
using Core.Entities;
using Core.Interfaces;
using Core.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly iDetailService service;

        public InvoiceDetailController(iDetailService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Details(int invoiceId)
        {
            var alldetails = this.service.invoiceDetails(invoiceId);
            var response = new ApiResponse<IEnumerable<InvoiceDetail>>(alldetails);

            return Ok(response);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> DetailById(int idDetail)
        {
            var detail = await this.service.invoiceDetail(idDetail);
            var response = new ApiResponse<InvoiceDetail>(detail);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddDetails([FromBody] List<InvoiceDetail> details)
        {
            var addDetails = await this.service.AddDetail(details);
            var response = new ApiResponse<bool>(addDetails);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDetails([FromBody] InvoiceDetail details)
        {
            var updateDetails = await this.service.UpdateDetail(details);
            var response = new ApiResponse<bool>(updateDetails);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetails(int id)
        {
            var delete = await this.service.DeleteDetail(id);
            var response = new ApiResponse<bool>(delete);

            return Ok(response);
        }
    }
}
