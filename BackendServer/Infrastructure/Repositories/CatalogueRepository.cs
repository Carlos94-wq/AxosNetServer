using Core.CustomEntities;
using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CatalogueRepository : ICatalogueRepository
    {
        private readonly ConnectionConfig options;

        public CatalogueRepository( IOptions<ConnectionConfig> options )
        {
            this.options = options.Value;
        }

        public IEnumerable<Product> GetProducts( ProductQueryFilters filters )
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var all = con.Query<Product>("spCatalogue", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 2,
                    ProductName = filters.ProductName,
                    InitPrice = filters.InitPrice,
                    EndPrice = filters.EndPrice
                });

                return all;
            }
        }

        public IEnumerable<Status> GetStatus()
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var all = con.Query<Status>("spCatalogue", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 3
                });

                return all;
            }
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var all = con.Query<Supplier>("spCatalogue", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 1
                });

                return all;
            }
        }
    }
}
