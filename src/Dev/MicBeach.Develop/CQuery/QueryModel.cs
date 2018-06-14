using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// Query Model
    /// </summary>
    public class QueryModel<T> : IQueryModel<T> where T : IQueryModel<T>
    {
        #region Propertys

        /// <summary>
        /// Object Name
        /// </summary>
        public static string QueryObjectName
        {
            get
            {
                return GetQueryObjectName();
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Get Object Name
        /// </summary>
        /// <returns></returns>
        static string GetQueryObjectName()
        {
            return QueryConfig.GetObjectName(typeof(T));
        }

        #endregion
    }
}
