using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Exceptions.AuthenticationExceptions
{

    /// <summary>
    /// Thrown when the user tries to login using an expired password.
    /// </summary>
    /// <seealso cref="AuthenticationException" />
    public class PasswordExpiredException : AuthenticationException
    {
        /// <summary>
        /// The expiry date time
        /// </summary>
        private readonly DateTime? _expiryDateTime;

        /// <summary>
        /// The message
        /// </summary>
        private static string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordExpiredException"/> class.
        /// </summary>
        public PasswordExpiredException() : base(_message)
        {
            _expiryDateTime = null;
            _message = "The password has expired. Please update your password before logging in.";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordExpiredException"/> class.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        public PasswordExpiredException(DateTime dateTime) : base(_message)
        {
            _expiryDateTime = dateTime;
            _message = "The password has expired on " + _expiryDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") 
                + ". Please update your password before logging in.";
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
