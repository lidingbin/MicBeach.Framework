using MicBeach.Develop.Command;
using MicBeach.Util.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.UnitOfWork
{
    /// <summary>
    /// IUnitOfWork
    /// </summary>
    public interface IUnitOfWork:IDisposable
    {
        /// <summary>
        /// Commit Command
        /// </summary>
        /// <returns></returns>
        CommitResult Commit();

        /// <summary>
        /// add command
        /// </summary>
        /// <param name="cmds">commands</param>
        void AddCommand(params ICommand[] cmds);

        /// <summary>
        /// command count
        /// </summary>
        int CommandCount { get;}
    }
}
