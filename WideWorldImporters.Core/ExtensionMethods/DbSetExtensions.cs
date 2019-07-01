using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WideWorldImporters.Core.ExtensionMethods
{

    /// <summary>
    /// Extension methods for DbSet
    /// </summary>
    public static class DbSetExtensions
    {

        /// <summary>
        /// Converts a DbSet to IQueryable which is not tracked
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="dbSet">DbSet</param>
        /// <returns></returns>
        public static IQueryable<T> AsNonTrackingQueryable<T>(this DbSet<T> dbSet) where T : class
        {
            return dbSet.AsQueryable().AsNoTracking();
        }

    }
    
}
