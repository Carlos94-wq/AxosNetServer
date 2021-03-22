using Core.CustomEntities;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.IServices
{
    public interface IAuthService
    {
        Task<User> Login(UserCredentials credentials);
        Task<User> Register(User user);
        Task<bool> RecoverPassword(string email, string NewPassword);
    }
}
