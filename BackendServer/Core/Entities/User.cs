using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public int StatusId { get; set; }

        public Status Status { get; set; }
        public List<Invoice> Invoices { get; set; }

        public User()
        {
            this.Status = new Status();
            this.Invoices = new List<Invoice>();
        }
    }
}
