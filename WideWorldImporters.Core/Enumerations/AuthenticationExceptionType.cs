using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Enumerations
{
    /// <summary>
    /// 
    /// </summary>
    public enum AuthenticationExceptionType {
        
        /// <summary>
        /// Invalid password
        /// </summary>
        InvalidPassword,
        
        /// <summary>
        /// Invalid username
        /// </summary>
        InvalidUsername,
        
        /// <summary>
        /// Password expired
        /// </summary>
        PasswordExpired
    };
}
