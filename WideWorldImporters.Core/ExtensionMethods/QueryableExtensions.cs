using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WideWorldImporters.Core.ExtensionMethods
{

    /// <summary>
    /// Extension methods for IQueryable
    /// </summary>
    public static class QueryableExtensions
    {

        /// <summary>
        /// Simple method to chunk a source IQueryable into smaller (more manageable) lists
        /// </summary>
        /// <param name="source">The large IQueryable to split</param>
        /// <param name="chunkSize">The maximum number of items each subset should contain</param>
        /// <returns>An IEnumerable of the original source IEnumerable in bite size chunks</returns>
        public static IEnumerable<IQueryable<TSource>> Chunk<TSource>(this IQueryable<TSource> source, int chunkSize)
        {
            for (int i = 0; i < source.Count(); i += chunkSize)
                yield return source.Skip(i).Take(chunkSize);
        }

        /// <summary>
        /// Simple method to chunk a source IQueryable into smaller (more manageable) lists
        /// </summary>
        /// <param name="source">The large IQueryable to split</param>
        public static IEnumerable<IQueryable<TSource>> Chunk<TSource>(this IQueryable<TSource> source)
        {
            int chunkSize = (int)Math.Sqrt(source.Count());

            for (int i = 0; i < source.Count(); i += chunkSize)
                yield return source.Skip(i).Take(chunkSize);
        }

        /// <summary>
        /// Converts a IQueryable to equivalent SQL Statement.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

            FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

            FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");

            FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

            PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = modelGenerator.ParseQuery(query.Expression);
            var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            
            var sql = modelVisitor.Queries.First().ToString()
                .Replace("\n", string.Empty)
                .Replace("\t", string.Empty)
                .Replace("\r", string.Empty);

            return sql;
        }


        /// <summary>
        /// Elements the exists at.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="position"></param>
        /// <returns></returns>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Not important in this situation")]
        public static async Task<bool> ElementExistsAtAsync<TEntity>(this IQueryable<TEntity> query, int position)
        {
            try
            {
                var val = await query.Skip(position).Take(1).FirstOrDefaultAsync();

                return val != null;

            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Smarts the take asynchronous.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="take">The take.</param>
        /// <returns></returns>
        public static async Task<Tuple<IQueryable<TEntity>, bool>> SmartTakeAsync<TEntity>(this IQueryable<TEntity> query, int take)
        {
            query = query.Take(take);

            var elementExistsAtLast = await query.ElementExistsAtAsync(take - 1);

            return new Tuple<IQueryable<TEntity>, bool>(query, elementExistsAtLast);

        }

    }
}
