using Core.CustomEntities;
using Core.Entities;
using Core.Interfaces;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InvoiceDetailRepository : IInvoiceDetailRepository
    {
        private readonly ConnectionConfig options;

        public InvoiceDetailRepository(IOptions<ConnectionConfig> options)
        {
            this.options = options.Value;
        }

        public IEnumerable<InvoiceDetail> invoiceDetails(int invoceId)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var details = con.Query<InvoiceDetail, Product, InvoiceDetail>("spInvoiceDetail", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 1,
                    InvoiceId = invoceId
                }, 
                splitOn: "ProductId",
                map:(i, p) => {
                    i.Product = p;
                    return i;
                });

                return details;
            }
        }

        public async Task<InvoiceDetail> invoiceDetail(int detailId)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var details = await con.QueryAsync<InvoiceDetail, Product, InvoiceDetail>("spInvoiceDetail", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 2,
                    DetailId = detailId
                },
                splitOn: "ProductId",
                map: (i, p) => {
                    i.Product = p;
                    return i;
                });

                return details.FirstOrDefault();
            }
        }

        public async Task<int> AddDetail(List<InvoiceDetail> details)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var add = 0;

                foreach (var item in details)
                {
                    add = await con.ExecuteAsync("spInvoiceDetail", commandType: CommandType.StoredProcedure, param: new
                    {
                        Action = 3,
                        InvoiceId = item.InvoiceId,
                        ProductId = item.ProductId,
                        Amount = item.Amount,
                        Price = item.Price
                    });
                }

                return add;
            }
        }

        public async Task<int> UpdateDetail(InvoiceDetail detail)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {

                var Update = await con.ExecuteAsync("spInvoiceDetail", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 4,
                    Amount = detail.Amount,
                    Price = detail.Price,
                    DetailId = detail.DetailId
                });

                return Update;
            }
        }

        public async Task<int> DeleteDetail(int detailId)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {

                var Update = await con.ExecuteAsync("spInvoiceDetail", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 4,
                    DetailId = detailId
                });

                return Update;
            }
        }
    }
}
