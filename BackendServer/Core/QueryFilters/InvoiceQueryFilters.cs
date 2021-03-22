using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class InvoiceQueryFilters
    {
        public int UserId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
