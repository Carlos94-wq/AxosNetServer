using Api.Responses;
using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CatalogueController : ControllerBase
    {
        private readonly ICatalogueRepository repository;

        public CatalogueController(ICatalogueRepository repository)
        {
            this.repository = repository;
        }


        [HttpGet]
        public IActionResult Suppliers()
        {
            var all = this.repository.GetSuppliers();
            var response = new ApiResponse<IEnumerable<Supplier>>(all);

            return Ok(response);
        }

        [HttpGet]
        public IActionResult Products([FromQuery] ProductQueryFilters filters)
        {
            var all = this.repository.GetProducts(filters);
            var response = new ApiResponse<IEnumerable<Product>>(all);

            return Ok(response);
        }

        [HttpGet]
        public IActionResult Status()
        {
            var all = this.repository.GetStatus();
            var response = new ApiResponse<IEnumerable<Status>>(all);

            return Ok(response);
        }
    }
}
