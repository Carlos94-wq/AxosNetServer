using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class InvoiceDetail
    {
        public int DetailId { get; set; }
        public int? InvoiceId { get; set; }
        public int? ProductId { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public bool ProductStatus { get; set; }

        public Product Product { get; set; }
        public InvoiceDetail()
        {
            this.Product = new Product();
        }
    }
}
