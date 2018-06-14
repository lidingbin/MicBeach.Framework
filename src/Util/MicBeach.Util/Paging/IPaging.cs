using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Paging
{
    /// <summary>
    /// Paging interface
    /// </summary>
    public interface IPaging<T> : IEnumerable<T>
    {
        #region Propertys

        /// <summary>
        /// Current Page
        /// </summary>
        long Page { get; }

        /// <summary>
        /// Page Size
        /// </summary>
        long PageSize { get; }

        /// <summary>
        /// Total Page
        /// </summary>
        long PageCount { get; }

        /// <summary>
        /// Total Data
        /// </summary>
        long TotalCount { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Paging Object Convert
        /// </summary>
        /// <typeparam name="TT">Target Object Type</typeparam>
        /// <returns>Target Paging Object</returns>
        IPaging<TT> ConvertTo<TT>();

        #endregion
    }
}
