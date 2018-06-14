using MicBeach.Util.Paging;
using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicBeach.Develop.Command;

namespace MicBeach.Develop.UnitOfWork
{
    /// <summary>
    /// UnitOfWork Manager
    /// </summary>
    public class UnitOfWork
    {
        [ThreadStatic]
        static IUnitOfWork current;

        /// <summary>
        /// current IUnitOfWork Object
        /// </summary>
        public static IUnitOfWork Current
        {
            get
            {
                return current;
            }
            internal set
            {
                current = value;
            }
        }

        #region Static Methods

        /// <summary>
        /// Register Command To UnityWork
        /// </summary>
        /// <param name="cmds">Commands</param>
        public static void RegisterCommand(params ICommand[] cmds)
        {
            Current?.AddCommand(cmds);
        }

        /// <summary>
        /// quer
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">query command</param>
        /// <returns>datas</returns>
        public static IEnumerable<T> Query<T>(ICommand cmd)
        {
            return CommandExecuteManager.Query<T>(cmd);
        }

        /// <summary>
        /// query datas with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>datas</returns>
        public static IPaging<T> QueryPaging<T>(ICommand cmd) where T : CommandEntity<T>
        {
            return CommandExecuteManager.QueryPaging<T>(cmd);
        }

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns>whether data is exist</returns>
        public static bool Query(ICommand cmd)
        {
            return CommandExecuteManager.Query(cmd);
        }

        /// <summary>
        /// query single data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>data</returns>
        public static T QuerySingle<T>(ICommand cmd)
        {
            return CommandExecuteManager.QuerySingle<T>(cmd);
        }

        /// <summary>
        /// create a new IUnitOrWork
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork Create()
        {
            return new DefaultUnitOfWork();
        }

        #endregion
    }
}
