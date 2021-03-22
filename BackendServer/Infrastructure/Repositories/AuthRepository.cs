using Core.CustomEntities;
using Core.Entities;
using Core.Interfaces;
using Dapper;
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
    public class AuthRepository : IAuthRepository
    {
        private readonly ConnectionConfig options;

        public AuthRepository(IOptions<ConnectionConfig> options)
        {
            this.options = options.Value;
        }

        /*
         *  no abro ni cierro conexiones  
         *  para eso se encarga el bloque using
         *  
         *  
         *  para acceso a base de datos estoy usando dapper como ORM
         */

        public async Task<User> Login(UserCredentials credentials)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                var login = await con.QueryAsync<User>("spAuth", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 1,
                    UserEmail = credentials.Email,
                    UserPassword = credentials.Password
                });

                return login.FirstOrDefault();
            }
        }

        public async Task<int> RecoverPassword(string email, string NewPassword)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                //verifica que el usuario exista
                var isExists = await con.QueryAsync<User>("spAuth", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 4,
                    UserEmail = email,
                });

                //si existe cambia la contrasenia
                if (isExists != null)
                {
                    var recover = await con.ExecuteAsync("spAuth", commandType: CommandType.StoredProcedure, param: new
                    {
                        Action = 3,
                        UserPassword = NewPassword,
                        UserId = isExists.FirstOrDefault().UserId
                    });

                    return recover;
                }
                else
                {
                    return 0; // en dado caso de que no retorna directamente un 0
                }

            }
        }

        public async Task<User> Register(User user)
        {
            using (IDbConnection con = new SqlConnection(this.options.DBINVOICES))
            {
                //crea el registro de un nuevo usuario
                var register = await con.ExecuteAsync("spAuth", commandType: CommandType.StoredProcedure, param: new
                {
                    Action = 2,
                    UserName = user.UserName,
                    UserLastName = user.UserLastName,
                    UserEmail = user.UserEmail,
                    UserPassword = user.UserPassword
                });

                if (register == 1)// si regresa 1 entoces hace el inisio de sesion
                {
                    UserCredentials credentials = new UserCredentials() { Email = user.UserEmail, Password = user.UserPassword };
                    var newUser = await this.Login(credentials);
                    return newUser;
                }
                else 
                {
                    return null;
                }
            }
        }
    }
}
