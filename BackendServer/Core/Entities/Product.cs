using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int SupplierId { get; set; }
    }
}
