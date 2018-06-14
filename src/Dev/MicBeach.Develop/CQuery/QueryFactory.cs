using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// Query Factory
    /// </summary>
    public static class QueryFactory
    {
        /// <summary>
        /// Create a new query instance
        /// </summary>
        /// <param name="objectName">object name</param>
        /// <returns>IQuery object</returns>
        public static IQuery Create(string objectName = "")
        {
            return new QueryInfo(objectName);
        }

        /// <summary>
        /// Create a new query instance
        /// </summary>
        /// <param name="filter">pagingfilter</param>
        /// <returns>IQuery object</returns>
        public static IQuery Create(PagingFilter filter)
        {
            var query = new QueryInfo();
            if (filter != null)
            {
                query.PagingInfo = filter;
            }
            return query;
        }

        /// <summary>
        /// Create a new query instance
        /// </summary>
        /// <typeparam name="T">query model</typeparam>
        /// <returns>IQuery object</returns>
        public static IQuery Create<T>() where T : IQueryModel<T>
        {
            return Create(QueryModel<T>.QueryObjectName);
        }

        /// <summary>
        /// Create a new query instance
        /// </summary>
        /// <typeparam name="T">query model</typeparam>
        /// <returns>IQuery object</returns>
        public static IQuery Create<T>(PagingFilter filter) where T : IQueryModel<T>
        {
            var query = Create(QueryModel<T>.QueryObjectName);
            if (filter != null)
            {
                query.PagingInfo = filter;
            }
            return query;
        }

        /// <summary>
        /// Create a new query instance
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="criteria">condition expression</param>
        /// <param name="objectName">object name</param>
        /// <returns>IQuery object</returns>
        public static IQuery Create<T>(Expression<Func<T, bool>> criteria, string objectName = "") where T : IQueryModel<T>
        {
            if (string.IsNullOrWhiteSpace(objectName))
            {
                objectName = QueryModel<T>.QueryObjectName;
            }
            IQuery query = Create(objectName);
            if (criteria != null)
            {
                query.And<T>(criteria);
            }
            return query;
        }
    }
}
