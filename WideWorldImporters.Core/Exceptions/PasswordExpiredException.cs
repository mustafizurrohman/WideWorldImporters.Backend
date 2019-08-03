using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.Annotations;

namespace WideWorldImporters.Core.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class PasswordExpiredException : Exception
    {
        private readonly DateTime? _expiryDateTime;

        /// <summary>
        /// 
        /// </summary>
        public PasswordExpiredException()
        {
            _expiryDateTime = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        public PasswordExpiredException(DateTime dateTime)
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
                    return "The password has expired on " + _expiryDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + ". Please update your password before logging in.";
                }
                
                return "The password has expired. Please update your password before logging in.";
                
            }
        }
    }
}
