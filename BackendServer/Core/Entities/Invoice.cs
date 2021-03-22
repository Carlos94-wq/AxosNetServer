using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
        public int SpplierId { get; set; }
        public int? StatusId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? InvoiceUpdate { get; set; }
        public DateTime? InvoiceDelete { get; set; }

        public List<InvoiceDetail> Details { get; set; }
        public Status Status { get; set; }
        public User user { get; set; }
        public Supplier Supplier { get; set; }

        public Invoice()
        {
            this.Details = new List<InvoiceDetail>();
            this.Status = new Status();
            this.user = new User();
        }
    }
}
