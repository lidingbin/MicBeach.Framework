using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Command
{
    /// <summary>
    ///  Command Execute Manager
    /// </summary>
    public static class CommandExecuteManager
    {
        static ICommandEngine cmdEngine;//command engine

        #region propertys

        /// <summary>
        /// get or set command engine
        /// </summary>
        public static ICommandEngine ExectEngine
        {
            get
            {
                return cmdEngine;
            }
            set
            {
                cmdEngine = value;
            }
        }

        #endregion

        #region methods

        #region execute

        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="commands">commands</param>
        /// <returns>return the execute data numbers</returns>
        internal static int Execute(IEnumerable<ICommand> commands)
        {
            if (commands == null || !commands.Any())
            {
                return 0;
            }
            return cmdEngine.Execute(commands.ToArray());
        }

        #endregion

        #region query

        /// <summary>
        /// execute query
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>queried datas</returns>
        internal static IEnumerable<T> Query<T>(ICommand cmd)
        {
            return cmdEngine.Query<T>(cmd);
        }

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>queried datas</returns>
        internal static IPaging<T> QueryPaging<T>(ICommand cmd) where T : CommandEntity<T>
        {
            return cmdEngine.QueryPaging<T>(cmd);
        }

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns>data is exist</returns>
        internal static bool Query(ICommand cmd)
        {
            return cmdEngine.Query(cmd);
        }

        /// <summary>
        /// query single data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>query data</returns>
        internal static T QuerySingle<T>(ICommand cmd)
        {
            return cmdEngine.QuerySingle<T>(cmd);
        }

        #endregion 

        #endregion
    }
}
