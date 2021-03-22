using Infrastructure.Serivices.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Serivices
{
    public class UriService : IUriService
    {
        private readonly string _baseUrl;

        public UriService(string baseUrl)
        {
            this._baseUrl = baseUrl;
        }

        public Uri PagedUrl(string actionUrl)
        {
            string baseUrl = $"{_baseUrl}{actionUrl}";
            return new Uri(baseUrl);
        }
    }
}
