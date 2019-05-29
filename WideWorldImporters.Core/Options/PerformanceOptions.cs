namespace WideWorldImporters.Core.Options
{

    /// <summary>
    /// PerformanceOptions
    /// </summary>
    public class PerformanceOptions
    {

        /// <summary>
        /// Boolean to indicate if response compression should be used
        /// </summary>
        public bool UseResponseCompression { get; set; } = true;

        /// <summary>
        /// Boolean to indicate if exception handling middleware should be used
        /// </summary>
        public bool UseExceptionHandlingMiddleware { get; set; } = true;

    }
}
