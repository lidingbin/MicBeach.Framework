using MicBeach.Develop.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.UnitOfWork
{
    /// <summary>
    /// default implements for IUnitOfWork
    /// </summary>
    public class DefaultUnitOfWork : IUnitOfWork
    {
        List<ICommand> commandList = new List<ICommand>();//command list

        /// <summary>
        /// instance a defaultunitofwork object
        /// </summary>
        internal DefaultUnitOfWork()
        {
            UnitOfWork.Current?.Dispose();
            UnitOfWork.Current = this;
        }

        /// <summary>
        /// command count
        /// </summary>
        public int CommandCount
        {
            get
            {
                return commandList?.Count ?? 0;
            }
        }

        #region Instance Methods

        /// <summary>
        /// Add Commands To UnitOfWork
        /// </summary>
        /// <param name="cmds">Commands</param>
        public void AddCommand(params ICommand[] cmds)
        {
            if (cmds == null)
            {
                return;
            }
            foreach (var cmd in cmds)
            {
                if (cmd == null)
                {
                    continue;
                }
                commandList.Add(cmd);
            }
        }

        /// <summary>
        /// Commit Command
        /// </summary>
        /// <returns></returns>
        public CommitResult Commit()
        {
            try
            {
                if (commandList.Count <= 0)
                {
                    return new CommitResult()
                    {
                        CommitCommandCount = 0,
                        ExecutedDataCount = 0
                    };
                }
                var exectCommandList = commandList.Select(c => c).ToList();
                bool beforeExecuteResult = ExecuteCommandBeforeExecute(exectCommandList);
                if (!beforeExecuteResult)
                {
                    throw new Exception("Any Command BeforeExecute Event Return Fail");
                }
                var result = CommandExecuteManager.Execute(exectCommandList);
                ExecuteCommandCallback(exectCommandList, result > 0);
                return new CommitResult()
                {
                    CommitCommandCount = exectCommandList.Count,
                    ExecutedDataCount = result
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Execute Command Before Execute
        /// </summary>
        /// <param name="cmds">command</param>
        static bool ExecuteCommandBeforeExecute(IEnumerable<ICommand> cmds)
        {
            if (cmds == null)
            {
                return false;
            }
            bool result = true;
            foreach (var cmd in cmds)
            {
                result = result && cmd.ExecuteBefore();
            }
            return result;
        }

        /// <summary>
        /// Execute Command Callback
        /// </summary>
        /// <param name="cmds">commands</param>
        void ExecuteCommandCallback(IEnumerable<ICommand> cmds, bool success)
        {
            Task.Run(() =>
            {
                foreach (var cmd in cmds)
                {
                    cmd?.ExecuteComplete(success);
                }
            });
        }

        #endregion

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            commandList.Clear();
        }
    }
}
