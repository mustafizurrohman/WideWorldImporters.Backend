using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Exceptions.AuthenticationExceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class PasswordExpiredException : AuthenticationException
    {
        private readonly DateTime? _expiryDateTime;

        private static string _message;

        /// <summary>
        /// 
        /// </summary>
        public PasswordExpiredException() : base(_message)
        {
            _expiryDateTime = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        public PasswordExpiredException(DateTime dateTime) : base(_message)
        {
            _expiryDateTime = dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Message
        {
            get
            {
                if (_expiryDateTime.HasValue)
                {
                    _message = "The password has expired on " + _expiryDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + ". Please update your password before logging in.";
                }

                _message = "The password has expired. Please update your password before logging in.";

                return _message;
            }
        }
    }
}
