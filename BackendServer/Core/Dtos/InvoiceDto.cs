using Core.Entities;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos
{ 
    [Table("INVOICE")]
    public class InvoiceDto
    {
        [Key]
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public string Comments { get; set; }
        public int SupplierId { get; set; }
        public int StatusId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? InvoiceUpdate { get; set; }
        public DateTime? InvoiceDelete { get; set; }
    }
}
