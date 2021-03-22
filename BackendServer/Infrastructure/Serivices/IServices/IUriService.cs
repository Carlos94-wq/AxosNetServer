using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Serivices.IServices
{
    public interface IUriService
    {
        Uri PagedUrl(string actionUrl);
    }
}
