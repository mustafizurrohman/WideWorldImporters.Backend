using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.AuthenticationProvider.Database;
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
                var newUser = await _authenticationService.AddUserAsync(username, password, email);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return BadRequest(ex.Message);
            }

            
        }

    }
}