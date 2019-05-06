using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Options
{

    /// <summary>
    /// PerformanceOptions
    /// </summary>
    public class PerformanceOptions
    {

        /// <summary>
        /// Bool to indicate if response compression should be used
        /// </summary>
        public bool UseResponseCompression { get; set; } = true;

        /// <summary>
        /// Bool to indicate if exception handling middleware should be used
        /// </summary>
        public bool UseExceptionHandlingMiddleware { get; set; } = true;

    }
}
