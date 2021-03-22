using Core.CustomEntities;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using Dapper;
using Dapper.Contrib.Extensions;
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
    public class InvoiceRepository : IinvoiceRepository
    {
        private readonly ConnectionConfig options;

        public InvoiceRepository(IOptions<ConnectionConfig> options)
        {
            this.options = options.Value;
        }

        public async Task<int> Add(InvoiceDto invoice)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {

                invoice.InvoiceDate = DateTime.Now;
                invoice.StatusId = 1;

                var add = await con.InsertAsync(invoice); //dapper contrib te regresa el id del registro

                return add;
            }
        }

        public async Task<int> Delete(int ididInvoice)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var delete = await con.ExecuteAsync("spInvoice", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 4,
                    InvoiceId = ididInvoice
                });

                return delete;
            }
        }

        public async Task<Invoice> invoice(int idInvoice)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var singleInvoice = await con.QueryAsync<Invoice, User, Status, Supplier ,Invoice>("spInvoice", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 5,
                    InvoiceId = idInvoice
                },
                map:( i, u, s, sp ) => {
                    i.user = u;
                    i.Status = s;
                    i.Supplier = sp;
                    return i;
                },
                splitOn: "UserId, StatusId, SupplierId");

                return singleInvoice.FirstOrDefault();
            }
        }

        public IEnumerable<Invoice> Invoices(InvoiceQueryFilters filters)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var Invoices = con.Query<Invoice, User, Status, Supplier, Invoice>("spInvoice", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 1,
                    UserId = filters.UserId,
                    InvoiceDate = filters.InvoiceDate,
                    SupplierId = filters.SupplierId
                },
                map: (i, u, s, sp) => {
                    i.user = u;
                    i.Status = s;
                    i.Supplier = sp;
                    return i;
                },
                splitOn: "UserId, StatusId, SupplierId");

                return Invoices;
            }
        }

        public async Task<int> Update(Invoice invoice)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var uppdate = await con.ExecuteAsync("spInvoice", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 3,
                    InvoiceId = invoice.InvoiceId,
                    Comments = invoice
                });

                return uppdate;
            }
        }
    }
}
