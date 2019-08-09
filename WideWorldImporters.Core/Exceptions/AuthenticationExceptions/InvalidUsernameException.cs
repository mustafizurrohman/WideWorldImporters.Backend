using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Exceptions.AuthenticationExceptions
{
    /// <summary>
    /// Thrown when the username is invalid
    /// </summary>
    /// <seealso cref="AuthenticationException" />
    public class InvalidUsernameException : AuthenticationException
    {
       
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidUsernameException"/> class.
        /// </summary>
        public InvalidUsernameException() : base("Invalid username.")
        {

        }
        
    }
}
