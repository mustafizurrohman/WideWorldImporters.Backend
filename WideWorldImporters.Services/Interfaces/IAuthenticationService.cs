using System.Threading.Tasks;
using WideWorldImporters.AuthenticationProvider.Database;

namespace WideWorldImporters.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Adds an user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Task<Users> AddUserAsync(string username, string password, string email, string apiKey);

        /// <summary>
        /// Adds an user with role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Task<Users> AddUserAndRoleAsync(string username, string password, string email, string role, string apiKey);

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isAdmin"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Task<Roles> AddRole(string role, bool isAdmin, string apiKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> AuthenticateUserAsync(string username, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Task<string> UpdatePasswordAsync(string username, string oldPassword, string newPassword, string apiKey);
    }
}