using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Core.Enumerations;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;
using WideWorldImporters.Services.Services.Base;

namespace WideWorldImporters.Services.Services
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceLifeTime(ServiceLifetime.Lifetime.Transient)]
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationServices"></param>
        public AuthenticationService(ApplicationServices applicationServices) : base(applicationServices)
        {
        }

        /// <summary>
        /// Adds an user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Users> AddUserAsync(string username, string password, string email)
        {
            if (!email.IsValidEmail())
            {
                throw new ArgumentException("Invalid email.");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username must be provided.");
            }

            
            if (!password.IsValidPassword())
            {
                throw new ArgumentException("Invalid password. It must contain at least 8 " +
                                            "characters, one upper case character, one lower case character " +
                                            "and one special character. ");
            }


            Users newUser = new Users()
            {
                // TODO: Let the database generate this for us
                UserId = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                PasswordExpiresOn = DateTime.Now.AddMonths(6),
                Email = email,
                Username = username,
                UsersRoles = null, 
                PasswordHash = password
            };

            await AuthDbContext.AddAsync(newUser);
            await AuthDbContext.SaveChangesAsync();

            var user = await AuthDbContext.Users
                .SingleAsync(usr => usr.Username == username);

            return user;
        }

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public async Task<Roles> AddRole(string role, bool isAdmin)
        {
            var existingRole = await AuthDbContext.Roles
                .SingleAsync(r => r.Role == role);

            if (existingRole != null)
            {
                throw new ArgumentException("Role already exists");
            }

            Roles newRole = new Roles()
            {
                RoleId = Guid.NewGuid(),
                Role = role,
                IsAdmin = isAdmin
            };

            await AuthDbContext.AddAsync(role);
            await AuthDbContext.SaveChangesAsync();

            var addedRole = await AuthDbContext.Roles.SingleAsync(r => r.Role == role);

            return addedRole;
        }
    }
}
