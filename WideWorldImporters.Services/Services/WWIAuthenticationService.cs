using CryptoHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Core.Enumerations;
using WideWorldImporters.Core.Exceptions.AuthenticationExceptions;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Core.Options;
using WideWorldImporters.Services.ServiceCollections;
using WideWorldImporters.Services.Services.Base;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Services.Services
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceLifeTime(Lifetime.Transient)]
    // ReSharper disable once InconsistentNaming
    public class WWIAuthenticationService : BaseService, Interfaces.IWWIAuthenticationService
    {
        private JWTKeySettings JwtOptions { get; }

        private IHostingEnvironment HostingEnvironment { get; }

        private bool IsInProduction => !HostingEnvironment.IsDevelopment();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices">Application Services</param>
        /// <param name="jwtOptions">JWT Options</param>
        /// <param name="hostingEnvironment">Hosting Environment</param>
        public WWIAuthenticationService(ApplicationServices applicationServices, IOptionsSnapshot<JWTKeySettings> jwtOptions,
            IHostingEnvironment hostingEnvironment) : base(applicationServices)
        {
            JwtOptions = jwtOptions.Value;
            HostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Adds an user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="email">Email</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        public async Task<Users> AddUserAsync(string username, string password, string email, string apiKey)
        {
            if (apiKey != JwtOptions.ApiKey && IsInProduction)
            {
                throw new AuthenticationException("Invalid API Key.", AuthenticationExceptionType.InvalidApiKey);
            }

            if (!email.IsValidEmail())
            {
                throw new ArgumentException("Invalid email.");
            }

            var emailExists = await AuthDbContext.Users.AnyAsync(usr => usr.Email == email);

            if (emailExists)
            {
                throw new ArgumentException("A user with that Email ID already exists.");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username must be provided.");
            }

            var usernameExists = await AuthDbContext.Users.AnyAsync(usr => usr.Username == username);

            if (usernameExists)
            {
                throw new ArgumentException("Username already exists.");
            }

            if (!password.IsValidPassword())
            {
                throw new AuthenticationException("Invalid password", AuthenticationExceptionType.InvalidPassword);
            }

            DateTime now = DateTime.Now;

            Users newUser = new Users()
            {
                // TODO: Let the database generate this for us
                UserId = Guid.NewGuid(),
                CreatedOn = now,
                PasswordExpiresOn = now.AddMonths(6),
                Email = email,
                Username = username,
                UsersRoles = null,
                PasswordHash = Crypto.HashPassword(password),
                PasswordCreatedOn = now
            };

            await AuthDbContext.AddAsync(newUser);
            await AuthDbContext.SaveChangesAsync();

            return newUser;
        }

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="isAdmin">Indicates if this role is an Admin role</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        public async Task<Roles> AddRole(string role, bool isAdmin, string apiKey)
        {
            if (apiKey != JwtOptions.ApiKey && IsInProduction)
            {
                throw new ArgumentException("Invalid API Key.");
            }

            var roleExists = await AuthDbContext.Roles
                .AnyAsync(r => r.Role == role);

            if (roleExists)
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

            return newRole;
        }

        /// <summary>
        /// Adds an user with role
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="email">Email</param>
        /// <param name="role">Role</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        public async Task<Users> AddUserAndRoleAsync(string username, string password, string email, string role, string apiKey)
        {
            if (apiKey != JwtOptions.ApiKey && IsInProduction)
            {
                throw new ArgumentException("Invalid API Key.");
            }

            var dbRole = await AuthDbContext.Roles.SingleOrDefaultAsync(r => r.Role == role);

            if (dbRole == null)
            {
                throw new ArgumentException("Invalid role");
            }

            var user = await AddUserAsync(username, password, email, apiKey);

            var newUserRole = new UsersRoles()
            {
                UsersRoleId = Guid.NewGuid(),
                UserId = user.UserId,
                RoleId = dbRole.RoleId
            };

            await AuthDbContext.AddAsync(newUserRole);
            await AuthDbContext.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Authenticates a user with a given username and password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        /// <exception cref="AuthenticationException">Authentication Exception</exception>
        public async Task<string> AuthenticateUserAsync(string username, string password)
        {
            return await AuthenticateUsernameAndPasswordAsync(username, password, true);
        }

        /// <summary>
        /// Updates the password for a username. 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Invalid username, password or API Key</exception>
        public async Task<string> UpdatePasswordAsync(string username, string oldPassword, string newPassword, string apiKey)
        {
            if (apiKey != JwtOptions.ApiKey && IsInProduction)
            {
                throw new ArgumentException("Invalid API Key.");
            }

            Users user;

            try
            {
                user = await VerifyUsernameAndPassword(username, oldPassword, false);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                throw;
            }

            if (oldPassword == newPassword)
            {
                throw new ArgumentException("New password must be different then then the old password.");
            }

            if (!newPassword.IsValidPassword())
            {
                throw new AuthenticationException("Invalid password", AuthenticationExceptionType.InvalidPassword);
            }

            user.PasswordCreatedOn = DateTime.Now;
            user.PasswordHash = Crypto.HashPassword(newPassword);
            user.PasswordExpiresOn = DateTime.Now.AddMonths(6);

            AuthDbContext.Update(user);
            await AuthDbContext.SaveChangesAsync();

            return newPassword;

        }

        /// <summary>
        /// Resets the password for a username. A new password is randomly generated and assigned.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        public async Task<string> ResetPasswordAsync(string username, string apiKey)
        {
            if (apiKey != JwtOptions.ApiKey && IsInProduction)
            {
                throw new ArgumentException("Invalid API Key.");
            }

            Users user = await AuthDbContext.Users
                .FirstOrDefaultAsync(usr => usr.Username == username);

            if (user == null)
            {
                throw new ArgumentException("Invalid username.");
            }

            var newPassword = Core.Helpers.StringHelpers.GetRandomPassword(8);

            DateTime now = DateTime.Now;

            user.PasswordCreatedOn = now;
            user.PasswordExpiresOn = now.AddMonths(6);
            user.PasswordHash = Crypto.HashPassword(newPassword);

            AuthDbContext.Update(user);
            await AuthDbContext.SaveChangesAsync();

            return newPassword;
        }

        #region -- Private Methods --

        /// <summary>
        /// Authenticates a username and password and generated a JWT if the credentials are correct
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="verifyPasswordValidity"></param>
        /// <returns></returns>
        /// <exception cref="AuthenticationException">Authentication Exception</exception>
        private async Task<string> AuthenticateUsernameAndPasswordAsync(string username, string password, bool verifyPasswordValidity)
        {
            Users user = await VerifyUsernameAndPassword(username, password, verifyPasswordValidity);

            return await GenerateJwtTokenAsync(user);
        }

        /// <summary>
        /// Verifies a username and password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="verifyPasswordValidity">Indicates if the validity of the password must be verified</param>
        /// <returns></returns>
        /// <exception cref="AuthenticationException">Authentication Exception</exception>
        private async Task<Users> VerifyUsernameAndPassword(string username, string password, bool verifyPasswordValidity)
        {
            Users user = await AuthDbContext.Users
                .Include(usr => usr.UsersRoles)
                .FirstOrDefaultAsync(usr => usr.Username == username);

            if (user == null)
            {
                // throw new InvalidUsernameException();
                throw new AuthenticationException("Invalid password", AuthenticationExceptionType.InvalidUsername);
            }

            bool validPassword = Crypto.VerifyHashedPassword(user.PasswordHash, password);

            if (!validPassword)
            {
                throw new AuthenticationException("Password is not valid.", AuthenticationExceptionType.InvalidPassword);
            }

            // TODO: Verify
            if (user.PasswordExpiresOn.GetValueOrDefault(DateTime.MinValue) < DateTime.Now && verifyPasswordValidity)
            {
                throw new AuthenticationException("Password expired.", AuthenticationExceptionType.PasswordExpired);
            }

            return user;
        }

        /// <summary>
        /// Generates a JWT for a given user
        /// </summary>
        /// <param name="user">User for whom the token must be generated.</param>
        /// <returns></returns>
        private async Task<string> GenerateJwtTokenAsync(Users user)
        {
            List<Guid> userRolesIds = user.UsersRoles.Select(x => x.RoleId).ToList();

            var userRoles = await AuthDbContext.Roles
                .Where(r => userRolesIds.Contains(r.RoleId))
                .ToListAsync();

            List<Claim> claimList = userRoles.Select(r => r.Role)
                .Select(currentRole => new Claim(ClaimTypes.Role, currentRole))
                .ToList();

            Claim nameClaim = new Claim(ClaimTypes.Name, user.Username);
            claimList.Add(nameClaim);

            Claim emailClaim = new Claim(ClaimTypes.Email, user.Email);
            claimList.Add(emailClaim);

            SigningCredentials signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey)),
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

        #endregion

    }
}
