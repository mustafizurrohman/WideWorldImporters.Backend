using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Exceptions.AuthenticationExceptions
{
    /// <summary>
    /// Thrown when the password is invalid
    /// </summary>
    /// <seealso cref="AuthenticationException" />
    public class InvalidPasswordException : AuthenticationException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPasswordException"/> class.
        /// </summary>
        public InvalidPasswordException() : base("Invalid password.")
        {

        }

    }
}
