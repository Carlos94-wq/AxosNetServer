using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public int StatusId { get; set; }
    }
}
