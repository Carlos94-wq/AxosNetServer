using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class ProductQueryFilters
    {
        public string ProductName { get; set; }
        public decimal? InitPrice { get; set; }
        public decimal? EndPrice { get; set; }
    }
}
