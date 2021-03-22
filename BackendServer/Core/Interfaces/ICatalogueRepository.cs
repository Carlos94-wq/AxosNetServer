using Core.Entities;
using Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface ICatalogueRepository
    {
        IEnumerable<Product> GetProducts(ProductQueryFilters filters);
        IEnumerable<Supplier> GetSuppliers();
        IEnumerable<Status> GetStatus();
    }
}
