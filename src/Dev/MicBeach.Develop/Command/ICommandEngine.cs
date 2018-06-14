using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Command
{
    /// <summary>
    /// command execute engine
    /// </summary>
    public interface ICommandEngine
    {
        #region execute methods

        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="cmds">commands</param>
        /// <returns>date numbers </returns>
        int Execute(params ICommand[] cmds);

        #endregion

        #region query methods

        /// <summary>
        /// execute query
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>datas</returns>
        IEnumerable<T> Query<T>(ICommand cmd);

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        IPaging<T> QueryPaging<T>(ICommand cmd) where T : CommandEntity<T>;

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns>data is exist</returns>
        bool Query(ICommand cmd);

        /// <summary>
        /// query a single data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>data</returns>
        T QuerySingle<T>(ICommand cmd);

        #endregion
    }
}
