using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;

namespace MicBeach.Util.Paging
{
    /// <summary>
    /// Paging Default Implement
    /// </summary>
    public class Paging<T> : IPaging<T>
    {
        #region fields

        private long _pageIndex = 1;//current page
        private long _pageSize = 1;//page size
        private long _pageCount = 0;//total page
        private long _totalCount = 0;//total data
        private T[] _items = new T[0];//datas

        #endregion

        #region Constructor

        /// <summary>
        /// Instance a paging object
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="totalCount">total data</param>
        /// <param name="items">datas</param>
        public Paging(long pageIndex, long pageSize, long totalCount, IEnumerable<T> items)
        {
            if (items != null)
            {
                this._pageIndex = pageIndex;
                this._pageSize = pageSize;
                this._totalCount = totalCount;
                this._items = items.ToArray();
                if (pageSize > 0)
                {
                    _pageCount = totalCount / pageSize;
                    if (totalCount % pageSize > 0)
                    {
                        _pageCount++;
                    }
                }
            }
        }

        #endregion

        #region Propertys

        /// <summary>
        /// Current Page
        /// </summary>
        public long Page
        {
            get { return _pageIndex; }
        }

        /// <summary>
        /// Page Size
        /// </summary>
        public long PageSize
        {
            get { return _pageSize; }
        }

        /// <summary>
        /// Total Page
        /// </summary>
        public long PageCount
        {
            get { return _pageCount; }
        }

        /// <summary>
        /// Total Datas
        /// </summary>
        public long TotalCount
        {
            get { return _totalCount; }
        }

        #endregion

        #region Functions

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                yield return _items[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Return a Empty Paging Object
        /// </summary>
        /// <returns></returns>
        public static Paging<T> EmptyPaging()
        {
            return new Paging<T>(1, 0, 0, null);
        }

        /// <summary>
        /// Paging Object Convert
        /// </summary>
        /// <typeparam name="TT">Target Object Type</typeparam>
        /// <returns>Target Paging Object</returns>
        public IPaging<TT> ConvertTo<TT>()
        {
            return new Paging<TT>(this.Page, this.PageSize, this.TotalCount, this.Select(c => c.MapTo<TT>()));
        }

        #endregion
    }
}
