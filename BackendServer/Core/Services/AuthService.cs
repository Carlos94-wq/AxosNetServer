using Core.CustomEntities;
using Core.Entities;
using Core.Interfaces;
using Core.Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository repository;

        public AuthService(IAuthRepository repository)
        {
            this.repository = repository;
        }
        
        public async Task<User> Login(UserCredentials credentials)
        {
            return await this.repository.Login(credentials);
        }

        public async Task<bool> RecoverPassword(string email, string NewPassword = null)
        {
            var isUpdate = await this.repository.RecoverPassword(email, NewPassword);
            return isUpdate > 0;
        }

        public async Task<User> Register(User user)
        {
            return await this.repository.Register(user);
        }
    }
}
