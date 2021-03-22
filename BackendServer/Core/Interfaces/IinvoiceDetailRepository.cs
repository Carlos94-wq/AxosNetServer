using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IInvoiceDetailRepository
    {
        Task<int> AddDetail(List<InvoiceDetail> details);
        Task<int> DeleteDetail(int detailId);
        Task<InvoiceDetail> invoiceDetail(int detailId);
        IEnumerable<InvoiceDetail> invoiceDetails(int invoceId);
        Task<int> UpdateDetail(InvoiceDetail detail);
    }
}