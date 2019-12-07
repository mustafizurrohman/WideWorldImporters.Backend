using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <param name="data">The data.</param>
        /// <exception cref="Exception"></exception>
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T data) where T : class
        {
            if (data == null)
            {
                return;
            }

            var context = dbSet.GetContext();
            var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);

            var t = typeof(T);
            List<PropertyInfo> keyFields = new List<PropertyInfo>();

            foreach (var propt in t.GetProperties())
            {
                var keyAttr = ids.Contains(propt.Name);
                if (keyAttr)
                {
                    keyFields.Add(propt);
                }
            }
            if (keyFields.Count <= 0)
            {
                throw new Exception($"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
            }
            var entities = dbSet.AsNoTracking().ToList();
            foreach (var keyField in keyFields)
            {
                var keyVal = keyField.GetValue(data);
                entities = entities.Where(p => p.GetType().GetProperty(keyField.Name).GetValue(p).Equals(keyVal)).ToList();
            }
            var dbVal = entities.FirstOrDefault();
            if (dbVal != null)
            {
                context.Entry(dbVal).CurrentValues.SetValues(data);
                context.Entry(dbVal).State = EntityState.Modified;
                return;
            }
            dbSet.Add(data);
        }

        /// <summary>
        /// Adds the or update.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <exception cref="Exception"></exception>
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, Expression<Func<T, object>> key, T data) where T : class
        {
            if (data == null)
            {
                return;
            }

            var context = dbSet.GetContext();
            var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);
            var t = typeof(T);
            var keyObject = key.Compile()(data);
            PropertyInfo[] keyFields = keyObject.GetType().GetProperties().Select(p => t.GetProperty(p.Name)).ToArray();
            if (keyFields == null)
            {
                throw new Exception($"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
            }
            var keyVals = keyFields.Select(p => p.GetValue(data));
            var entities = dbSet.AsNoTracking().ToList();
            int i = 0;
            foreach (var keyVal in keyVals)
            {
                entities = entities.Where(p => p.GetType().GetProperty(keyFields[i].Name).GetValue(p).Equals(keyVal)).ToList();
                i++;
            }
            if (entities.Any())
            {
                var dbVal = entities.FirstOrDefault();
                var keyAttrs =
                    data.GetType().GetProperties().Where(p => ids.Contains(p.Name)).ToList();
                if (keyAttrs.Any())
                {
                    foreach (var keyAttr in keyAttrs)
                    {
                        keyAttr.SetValue(data,
                            dbVal.GetType()
                                .GetProperties()
                                .FirstOrDefault(p => p.Name == keyAttr.Name)
                                .GetValue(dbVal));
                    }
                    context.Entry(dbVal).CurrentValues.SetValues(data);
                    context.Entry(dbVal).State = EntityState.Modified;
                    return;
                }
            }
            dbSet.Add(data);
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbSet">The database set.</param>
        /// <returns></returns>
        public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
        {
            return (DbContext)dbSet
                .GetType().GetTypeInfo()
                .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dbSet);
        }

    }
    
}
