using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Exceptions.PasswordExceptions
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class InvalidPasswordException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPasswordException"/> class.
        /// </summary>
        public InvalidPasswordException()
        {

        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return "Password does not meet the password policy. Password must contain " +
                       "at least 8 characters, one upper case character, one lower case character " +
                       "and one special character. ";
            }
        }
    }
}
