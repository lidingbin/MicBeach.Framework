using MicBeach.Develop.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DB
{
    /// <summary>
    /// database execute engine
    /// </summary>
    public interface IDbEngine
    {
        #region execute

        /// <summary>
        /// execute command
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">server</param>
        /// <param name="cmds">command</param>
        /// <returns>data numbers</returns>
        int Execute(ServerInfo server, params ICommand[] cmds);

        #endregion

        #region query

        /// <summary>
        /// query data list
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">database server</param>
        /// <param name="cmd">command</param>
        /// <returns>data list</returns>
        IEnumerable<T> Query<T>(ServerInfo server, ICommand cmd);

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">databse server</param>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        IEnumerable<T> QueryPaging<T>(ServerInfo server, ICommand cmd);

        /// <summary>
        /// query data list offset the specified numbers
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">database server</param>
        /// <param name="cmd">command</param>
        /// <param name="offsetNum">offset num</param>
        /// <param name="size">query size</param>
        /// <returns></returns>
        IEnumerable<T> QueryOffset<T>(ServerInfo server, ICommand cmd,int offsetNum=0,int size=int.MaxValue);

        /// <summary>
        /// determine whether data has existed
        /// </summary>
        /// <param name="server">server</param>
        /// <param name="cmd">command</param>
        /// <returns>data has existed</returns>
        bool Query(ServerInfo server, ICommand cmd);

        /// <summary>
        /// query single value
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">database server</param>
        /// <param name="cmd">command</param>
        /// <returns>query data</returns>
        T QuerySingle<T>(ServerInfo server, ICommand cmd);

        #endregion
    }
}
