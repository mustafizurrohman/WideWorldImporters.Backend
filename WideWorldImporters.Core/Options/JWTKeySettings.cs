using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Options
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class JWTKeySettings
    {
        /// <summary>
        /// Expiry in days
        /// </summary>
        public int ExpireInDays { get; set; }

        /// <summary>
        /// Signing key
        /// </summary>
        public string SigningKey { get; set; }

        /// <summary>
        /// Api Key
        /// </summary>
        public string ApiKey { get; set; }
    }
}
