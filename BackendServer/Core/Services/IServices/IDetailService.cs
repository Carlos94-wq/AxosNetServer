using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.IServices
{
    public interface iDetailService
    {
        Task<bool> AddDetail(List<InvoiceDetail> details);
        Task<bool> DeleteDetail(int detailId);
        Task<bool> UpdateDetail(InvoiceDetail detail);
        Task<InvoiceDetail> invoiceDetail(int detailId);
        IEnumerable<InvoiceDetail> invoiceDetails(int invoceId);
       
    }
}
