using Api.Responses;
using AutoMapper;
using Core.CustomEntities;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using Core.Services.IServices;
using Infrastructure.Serivices.IServices;
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
    public class InvoiceController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInvoiceService service;
        private readonly IUriService uri;

        public InvoiceController(IMapper mapper, IInvoiceService service, IUriService uri)
        {
            this.mapper = mapper;
            this.service = service;
            this.uri = uri;
        }

        [HttpGet]
        public IActionResult GetInvoices([FromQuery] InvoiceQueryFilters filters) 
        {
            var All = this.service.Invoices(filters);
            var metada = new Metadata
            {
                TotalCount = All.TotalCount,
                PageSize = All.PageSize,
                CurrentPage = All.CurrentPage,
                TotalPages = All.TotalPages,
                HasNextPage = All.HasNextPage,
                HasPreviousPage = All.HasPreviousPage,
                NextPageUrl = uri.PagedUrl(Url.RouteUrl(nameof(GetInvoices))).ToString(),
                PreviousPageUrl = uri.PagedUrl(Url.RouteUrl(nameof(GetInvoices))).ToString()
            };
            var response = new ApiResponse<IEnumerable<Invoice>>(All)
            {
                Metadata = metada
            };

            if (All.Count == 0)
            {
                return BadRequest();
            }
            else {
                return Ok(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InvoiceDto invoice) 
        {
            var add = await this.service.Add(invoice);
            var repsonse = new ApiResponse<int>(add);

            return Ok(repsonse);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InvoiceDto invoice)
        {
            var domain = this.mapper.Map<Invoice>(invoice);
            var update = await this.service.Update(domain);

            var repsonse = new ApiResponse<bool>(update);
            return Ok(repsonse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var delete = await this.service.Delete(id);
            var repsonse = new ApiResponse<bool>(delete);

            return Ok(repsonse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> get(int id)
        {
            var data = await this.service.invoice(id);
            var repsonse = new ApiResponse<Invoice>(data);

            return Ok(repsonse);
        }
    }
}
