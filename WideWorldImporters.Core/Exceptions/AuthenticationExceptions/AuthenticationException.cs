using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Exceptions.AuthenticationExceptions
{
    /// <summary>
    /// This exception is thrown when authentication fails.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public abstract class AuthenticationException : Exception
    {

        private readonly string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
        /// </summary>
        public AuthenticationException(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return _message;
            }
        }


    }
}
