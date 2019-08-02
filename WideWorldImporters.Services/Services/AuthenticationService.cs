using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using CryptoHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Core.Enumerations;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Core.Options;
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
        private JWTKeySettings JwtOptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationServices"></param>
        /// <param name="jwtOptions"></param>
        public AuthenticationService(ApplicationServices applicationServices, IOptions<JWTKeySettings> jwtOptions) : base(applicationServices)
        {
            JwtOptions = jwtOptions.Value;
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

            var emailExists = await AuthDbContext.Users.FirstOrDefaultAsync(usr => usr.Email == email);

            if (emailExists != null)
            {
                throw new ArgumentException("A user with that Email ID already exists.");
            }

            var userExists = await AuthDbContext.Users.FirstOrDefaultAsync(usr => usr.Username == username);

            if (userExists != null)
            {
                throw new ArgumentException("User with that name already exists.");
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
                PasswordHash = Crypto.HashPassword(password)
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
                .FirstOrDefaultAsync(r => r.Role == role);

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

            await AuthDbContext.AddAsync(newRole);
            await AuthDbContext.SaveChangesAsync();

            var addedRole = await AuthDbContext.Roles.SingleAsync(r => r.Role == role);

            return addedRole;
        }

        /// <summary>
        /// Adds an user with role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Users> AddUserAndRoleAsync(string username, string password, string email, string role)
        {
            var user = await AddUserAsync(username, password, email);

            var dbRole = await AuthDbContext.Roles.FirstOrDefaultAsync(r => r.Role == role);

            if (dbRole == null)
            {
                throw new  ArgumentException("Invalid role");
            }

            var newUserRole = new UsersRoles()
            {
                UsersRoleId = Guid.NewGuid(),
                UserId = user.UserId,
                RoleId = dbRole.RoleId
            };

            await AuthDbContext.AddAsync(newUserRole);
            await AuthDbContext.SaveChangesAsync();

            var newUser = await AuthDbContext.Users.Include(usr => usr.UsersRoles)
                .Where(usr => usr.UserId == user.UserId)
                .SingleAsync();

            return newUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            Users user = await AuthDbContext.Users
                .Include(usr => usr.UsersRoles)
                .FirstOrDefaultAsync(usr => usr.Username == username);

            if (user == null)
            {
                throw new ArgumentException("Invalid username.");
            }

            bool validPassword = Crypto.VerifyHashedPassword(user.PasswordHash, password);

            if (!validPassword)
            {
                throw new ArgumentException("Invalid password.");
            }

            
            List<Guid> userRolesIds = user.UsersRoles.Select(x => x.RoleId).ToList();

            var userRoles = await AuthDbContext.Roles
                .Where(r => userRolesIds.Contains(r.RoleId))
                .ToListAsync();

            List<Claim> claimList = userRoles.Select(r => r.Role)
                .Select(currentRole => new Claim(ClaimTypes.Role, currentRole))
                .ToList();

            Claim name = new Claim(ClaimTypes.Name, user.Username);
            claimList.Add(name);

            Claim email = new Claim(ClaimTypes.Email, user.Email);
            claimList.Add(email);
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtOptions.SigningKey)), 
                SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.Now.Add(TimeSpan.FromDays(JwtOptions.ExpireInDays)),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(securityTokenDescriptor);
            string token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return token;

        }
    }
}
