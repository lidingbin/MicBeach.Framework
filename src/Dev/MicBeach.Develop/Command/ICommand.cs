using MicBeach.Develop.CQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Command
{
    /// <summary>
    /// command interface
    /// </summary>
    public interface ICommand
    {
        #region Propertys

        /// <summary>
        /// Command Text
        /// </summary>
        string CommandText { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        dynamic Parameters { get; set; }

        /// <summary>
        /// ObjectName
        /// </summary>
        string ObjectName { get; set; }

        /// <summary>
        /// ObjectKeys
        /// </summary>
        SortedSet<string> ObjectKeys { get; set; }

        /// <summary>
        /// ObjectKeyValues
        /// </summary>
        SortedDictionary<string, dynamic> ObjectKeyValues { get; set; }

        /// <summary>
        /// ServerKeys
        /// </summary>
        SortedSet<string> ServerKeys { get; set; }

        /// <summary>
        /// ServerKey Values
        /// </summary>
        SortedDictionary<string, dynamic> ServerKeyValues { get; set; }

        /// <summary>
        /// Execute Mode
        /// </summary>
        CommandExecuteMode ExecuteMode { get; set; }

        /// <summary>
        /// Query
        /// </summary>
        IQuery Query { get; set; }

        /// <summary>
        /// Operate
        /// </summary>
        OperateType Operate { get; set; }

        /// <summary>
        /// Fields
        /// </summary>
        IEnumerable<string> Fields { get; set; }

        /// <summary>
        /// Verify Result Method
        /// </summary>
        Func<int, bool> VerifyResult { get; set; }

        /// <summary>
        /// Success Callback
        /// </summary>
        event ExecuteCommandCallback SuccessCallback;

        /// <summary>
        /// Failed Callback
        /// </summary>
        event ExecuteCommandCallback FailedCallback;

        /// <summary>
        /// Before Execute
        /// </summary>
        event BeforeExecute BeforeExecute;

        /// <summary>
        /// Before Execute Request
        /// </summary>
        BeforeExecuteRequest BeforeRequest { get; set; }

        /// <summary>
        /// callback request
        /// </summary>
        CommandCallbackRequest CallbackRequest { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Execute Complete
        /// </summary>
        /// <param name="success">success</param>
        void ExecuteComplete(bool success);

        /// <summary>
        /// execute before
        /// </summary>
        /// <returns></returns>
        bool ExecuteBefore();

        #endregion
    }
}
