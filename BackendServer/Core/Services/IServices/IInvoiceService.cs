using Core.CustomEntities;
using Core.Dtos;
using Core.Entities;
using Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.IServices
{
    public interface IInvoiceService
    {
        PagedList<Invoice> Invoices(InvoiceQueryFilters filters);
        Task<Invoice> invoice(int idInvoice);
        Task<int> Add(InvoiceDto invoice);
        Task<bool> Update(Invoice invoice);
        Task<bool> Delete(int ididInvoice);
    }
}
