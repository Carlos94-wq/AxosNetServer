using Core.CustomEntities;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using Core.Services.IServices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly PaginationOptions options;
        private readonly IOptions<PaginationOptions> options1;
        private readonly IinvoiceRepository repository;
        private readonly IInvoiceDetailRepository detailRepository;

        //constructor de la clase
        public InvoiceService(
            IOptions<PaginationOptions> options, 
            IinvoiceRepository repository, 
            IInvoiceDetailRepository detailRepository
        )
        {
            this.options = options.Value;
            options1 = options;
            this.repository = repository;
            this.detailRepository = detailRepository;
        }

        public async Task<int> Add(InvoiceDto invoice)
        {
            return await this.repository.Add(invoice);
        }

        public async Task<bool> Delete(int ididInvoice)
        {
            var delete = await this.repository.Delete(ididInvoice);
            return delete > 0;
        }

        public async Task<Invoice> invoice(int idInvoice)
        {
            return await this.repository.invoice(idInvoice);
        }

        public async Task<bool> Update(Invoice invoice)
        {
            var delete = await this.repository.Update(invoice);
            return delete > 0;
        }

        public PagedList<Invoice> Invoices(InvoiceQueryFilters filter)
        {
            filter.PageNumber = filter.PageNumber == 0 ? this.options.DefaultPageNumber : filter.PageNumber;
            filter.PageSize = filter.PageSize == 0 ? this.options.DefaultPageSize : filter.PageSize;

            var All = this.repository.Invoices(filter);
            return PagedList<Invoice>.Create(All, filter.PageNumber, filter.PageSize);
        }
    }
}
