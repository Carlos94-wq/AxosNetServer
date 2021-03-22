using Core.Dtos;
using Core.Entities;
using Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IinvoiceRepository
    {
        IEnumerable<Invoice> Invoices(InvoiceQueryFilters filters);
        Task<Invoice> invoice(int idInvoice);
        Task<int> Add(InvoiceDto invoice);
        Task<int> Update(Invoice invoice);
        Task<int> Delete(int ididInvoice);
    }
}
