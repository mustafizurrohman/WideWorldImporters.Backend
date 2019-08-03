using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.Exceptions;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.API.Controllers
{
    /// <summary>
    /// Controller for Authentication
    /// </summary>
    public class AuthenticationController : BaseAPIController
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices"></param>
        /// <param name="authenticationService"></param>
        public AuthenticationController(ApplicationServices applicationServices,
            IAuthenticationService authenticationService)
            : base(applicationServices)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await AuthDbContext.Users.ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("users")]
        public async Task<IActionResult> AddUser(string username, string password, string email)
        {
            try
            {
                var newUser = await _authenticationService.AddUserAsync(username, password, email, string.Empty);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("users/role")]
        public async Task<IActionResult> AddUserWithRole(string username, string password, string email, string role)
        {
            try
            {
                var newUser = await _authenticationService.AddUserAndRoleAsync(username, password, email, role, string.Empty);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPost("role")]
        public async Task<IActionResult> AddRole(string role, bool isAdmin)
        {
            try
            {
                var newRole = await _authenticationService.AddRole(role, isAdmin, string.Empty);
                return Ok(newRole);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
           var roles = await AuthDbContext.Roles.ToListAsync();
           return Ok(roles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("jwt")]
        public async Task<IActionResult> GetJwtToken(string username, string password)
        {
            try
            {
                var token = await _authenticationService.AuthenticateUserAsync(username, password);
                return Ok(token);
            }
            catch (ArgumentException ex)
            {
                Logger.LogError("Authentication failed for '" + username + "'. " + Environment.NewLine + ex);
                return Unauthorized("Invalid username or password");
            }
            catch (PasswordExpiredException ex)
            {
                Logger.LogError(ex.Message);
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                var password = await _authenticationService.UpdatePasswordAsync(username, oldPassword, newPassword, string.Empty);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return Unauthorized(ex.Message);
            }
        }

    }
}