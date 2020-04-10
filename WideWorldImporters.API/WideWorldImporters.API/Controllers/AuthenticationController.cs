using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WideWorldImporters.API.ActionFilters;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.Exceptions.AuthenticationExceptions;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.API.Controllers
{
    /// <summary>
    /// Controller for Authentication
    /// </summary>
    [Insecure]
    [Testing]
    public class AuthenticationController : BaseAPIController
    {
        private readonly IWWIAuthenticationService _authenticationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices"></param>
        /// <param name="authenticationService"></param>
        public AuthenticationController(ApplicationServices applicationServices,
            IWWIAuthenticationService authenticationService)
            : base(applicationServices)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Gets a list of uesrs
        /// Warning: Insecure. Not Production ready.
        /// This should NEVER be a part of Public API
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await AuthDbContext.Users
                .Select(usr => new { usr.Username, usr.Email })
                .ToListAsync();

            return Ok(users);
        }

        /// <summary>
        /// Adds an user 
        /// Warning: Insecure. Not Production ready.
        /// In production we will make sure that this request is made by an admin
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("users")]
        [ProducesResponseType(typeof(Users), (int)HttpStatusCode.OK)]
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
        /// Adds an user with a role
        /// Warning: Insecure. Not Production ready.
        /// In production we will make sure that this request is made by an admin
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost("users/role")]
        [ProducesResponseType(typeof(Users), (int)HttpStatusCode.OK)]
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
        /// Adds a role
        /// Warning: Insecure. Not Production ready.
        /// This should NEVER be a part of Public API
        /// </summary>
        /// <param name="role"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPost("role")]
        [ProducesResponseType(typeof(Roles), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddRole(string role, bool isAdmin)
        {
            try
            {
                var newRole = await _authenticationService.AddRole(role, isAdmin, string.Empty);
                return Ok(newRole);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get list of roles
        /// Warning: Insecure. Not Production ready.
        /// This should NEVER be a part of Public API
        /// </summary>
        /// <returns></returns>
        [HttpGet("roles")]
        [ProducesResponseType(typeof(Roles), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoles()
        {
            var roles1 = await AuthDbContext.Roles.ToListAsync();
            List<Roles> roles2 = await AuthDbContext.Roles.ToListAsync();
            return Ok(roles1);
        }

        /// <summary>
        /// Authenticate a user with username and password.
        /// Warning: Insecure. Not Production ready.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AuthenticationException">Authentication Exception</exception>
        [HttpGet("jwt")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetJwtToken(string username, string password)
        {
            try
            {
                var token = await _authenticationService.AuthenticateUserAsync(username, password);
                return Ok(token);
            }
            catch (AuthenticationException ex)
            {
                var type1 = ex.ExceptionType;
                var type2 = ex.GetExceptionType();
                Logger.LogError("Authentication failed for '" + username + "'. " + Environment.NewLine + ex);
                return Unauthorized("Authentication failed.");
            }
        }

        /// <summary>
        /// Update a password when correct credentials are provided.
        /// Warning: Insecure. Not Production ready.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPut("password/update")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
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

        /// <summary>
        /// Resets a password for a given username.
        /// Could be used only by an Admin to generate a new password for a user.
        /// Warning: Insecure. Not Production ready.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("password/reset")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ResetPassword(string username)
        {
            try
            {
                var password = await _authenticationService.ResetPasswordAsync(username, string.Empty);
                return Ok(password);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return Unauthorized(ex.Message);
            }
        }



    }
}