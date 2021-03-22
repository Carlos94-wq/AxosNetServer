using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.IServices
{
    public interface IEmailSender
    {
        Task SenderEmail(string email, string subject, string body);
    }
}
