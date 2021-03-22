using Core.CustomEntities;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Login(UserCredentials credentials);
        Task<User> Register(User user);
        Task<int> RecoverPassword(string email, string NewPassword); 
    }
}
