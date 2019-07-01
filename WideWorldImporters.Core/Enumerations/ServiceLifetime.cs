namespace WideWorldImporters.Core.Enumerations
{

    /// <summary>
    /// Service LifeTime attribute 
    /// </summary>
    public class ServiceLifetime
    {
        /// <summary>
        /// Enum of service lifetime
        /// </summary>
        public enum Lifetime
        {
            /// <summary>
            /// Singleton service
            /// </summary>
            Singleton,

            /// <summary>
            /// Scoped service
            /// </summary>
            Scoped,
            
            /// <summary>
            /// Transient service
            /// </summary>
            Transient
        };

    }
}
