using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Core.Enumerations;

namespace WideWorldImporters.Core.Exceptions.AuthenticationExceptions
{
    /// <summary>
    /// This exception is thrown when authentication fails.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class AuthenticationException : Exception
    {
        /// <summary>
        /// The message
        /// </summary>
        private static string _message;

        /// <summary>
        /// The exception type
        /// </summary>
        private static AuthenticationExceptionType _exceptionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
        /// </summary>
        public AuthenticationException(string message, AuthenticationExceptionType exceptionType)
        {
            _message = message;
            _exceptionType = exceptionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationException"/> class.
        /// </summary>
        public AuthenticationException()
        {
            _message = "An Authentication Exception occured.";
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

        /// <summary>
        /// Gets the type of the exception.
        /// </summary>
        /// <returns></returns>
        public AuthenticationExceptionType GetExceptionType()
        {
            return _exceptionType;
        }

        /// <summary>
        /// Gets the type of the exception.
        /// </summary>
        public AuthenticationExceptionType ExceptionType => _exceptionType;


    }

    
}
