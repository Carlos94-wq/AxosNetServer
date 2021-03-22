using Core.Entities;
using Core.Interfaces;
using Core.Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class DetailService : iDetailService
    {
        private readonly IInvoiceDetailRepository invoice;

        public DetailService(IInvoiceDetailRepository invoice)
        {
            this.invoice = invoice;
        }

        public async Task<bool> AddDetail(List<InvoiceDetail> details)
        {
            var delete = await this.invoice.AddDetail(details);
            return delete > 0;
        }

        public async Task<bool> DeleteDetail(int detailId)
        {
            var delete = await this.invoice.DeleteDetail(detailId);
            return delete > 0;
        }

        public async Task<InvoiceDetail> invoiceDetail(int detailId)
        {
            return await this.invoice.invoiceDetail(detailId);
        }

        public IEnumerable<InvoiceDetail> invoiceDetails(int invoceId)
        {
            return this.invoice.invoiceDetails(invoceId);
        }

        public async Task<bool> UpdateDetail(InvoiceDetail detail)
        {
            var update = await this.invoice.UpdateDetail(detail);
            return update > 0;
        }
    }
}
