using MicBeach.Develop.Command;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Develop.DataAccess
{
    /// <summary>
    /// data access
    /// </summary>
    public interface IDataAccess<T>
    {
        #region add

        /// <summary>
        /// add data
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>icommand</returns>
        ICommand Add(T obj);

        /// <summary>
        /// add data list
        /// </summary>
        /// <param name="objList">object list</param>
        /// <returns>icommand list</returns>
        List<ICommand> Add(IEnumerable<T> objList);

        #endregion

        #region edit data

        /// <summary>
        /// edit data
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="query">query object</param>
        /// <returns>ICommand object</returns>
        ICommand Modify(T obj, IQuery query);

        /// <summary>
        /// edit data with expression
        /// </summary>
        /// <param name="modifyExpression">modify expression</param>
        /// <param name="query">query object</param>
        /// <returns>ICommand object</returns>
        ICommand Modify(IModify modifyExpression, IQuery query);

        #endregion

        #region delete data

        /// <summary>
        /// delete data
        /// </summary>
        /// <param name="query">delete query</param>
        /// <returns>ICommand object</returns>
        ICommand Delete(IQuery query);

        #endregion

        #region query data

        /// <summary>
        /// query data
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data</returns>
        T Get(IQuery query);

        /// <summary>
        /// query data list
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data list</returns>
        List<T> GetList(IQuery query);

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data paging</returns>
        IPaging<T> GetPaging(IQuery query);

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>whether data is exist</returns>
        bool Exist(IQuery query);

        /// <summary>
        /// get max value
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>max value</returns>
        DT Max<DT>(IQuery query);

        /// <summary>
        /// get minvalue
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>minvalue</returns>
        DT Min<DT>(IQuery query);

        /// <summary>
        /// caculate sum
        /// </summary>
        /// <typeparam name="DT">data value</typeparam>
        /// <param name="query">query value</param>
        /// <returns>caculated value</returns>
        DT Sum<DT>(IQuery query);

        /// <summary>
        /// caculate average
        /// </summary>
        /// <typeparam name="DT">data type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>average value</returns>
        DT Avg<DT>(IQuery query);

        /// <summary>
        /// caculate count
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>data count</returns>
        long Count(IQuery query);

        #endregion
    }
}
