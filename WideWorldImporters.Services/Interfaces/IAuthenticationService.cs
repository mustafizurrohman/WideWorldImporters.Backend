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
        /// <returns></returns>
        Task<Users> AddUserAsync(string username, string password, string email);

        /// <summary>
        /// Adds an user with role
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<Users> AddUserAndRoleAsync(string username, string password, string email, string role);

        /// <summary>
        /// Adds a role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        Task<Roles> AddRole(string role, bool isAdmin);
    }
}