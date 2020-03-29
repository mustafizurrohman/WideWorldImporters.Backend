namespace WideWorldImporters.Core.Enumerations
{
    /// <summary>
    /// Enum for type of Authentication Exception
    /// </summary>
    public enum AuthenticationExceptionType
    {

        /// <summary>
        /// Unspecified
        /// </summary>
        Unspecified,

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
        PasswordExpired,

        /// <summary>
        /// Invalid API key
        /// </summary>
        InvalidApiKey
    };
}
