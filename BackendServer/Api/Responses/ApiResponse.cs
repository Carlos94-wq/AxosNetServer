using Core.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Responses
{
    public class ApiResponse<T>
    {
        public T data { get; set; }
        public Metadata Metadata{ get; set; }

        public ApiResponse(T data)
        {
            this.data = data;
        }
    }
}
