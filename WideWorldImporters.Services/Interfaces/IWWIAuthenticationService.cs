using System.Threading.Tasks;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.Exceptions.AuthenticationExceptions;

namespace WideWorldImporters.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IWWIAuthenticationService
    {

        /// <summary>
        /// Adds an user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="email">Email</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        Task<Users> AddUserAsync(string username, string password, string email, string apiKey);

        /// <summary>
        /// Adds an user with role
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="email">Email</param>
        /// <param name="role">Role</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        Task<Users> AddUserAndRoleAsync(string username, string password, string email, string role, string apiKey);

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role">Role</param>
        /// <param name="isAdmin">Indicates if this role is an Admin role</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        Task<Roles> AddRole(string role, bool isAdmin, string apiKey);

        /// <summary>
        /// Authenticates a user with a given username and password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        /// <exception cref="AuthenticationException">Authentication Exception</exception>
        Task<string> AuthenticateUserAsync(string username, string password);

        /// <summary>
        /// Updates the password for a username. 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        Task<string> UpdatePasswordAsync(string username, string oldPassword, string newPassword, string apiKey);

        /// <summary>
        /// Resets the password for a username. A new password is randomly generated and assigned.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="apiKey">API Key</param>
        /// <returns></returns>
        Task<string> ResetPasswordAsync(string username, string apiKey);
    }
}