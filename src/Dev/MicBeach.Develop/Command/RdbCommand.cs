using MicBeach.Develop.CQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Command
{
    /// <summary>
    /// command for RDB
    /// </summary>
    public class RdbCommand : ICommand
    {
        private RdbCommand()
        { }

        #region fields

        /// <summary>
        /// 命令语句
        /// </summary>
        string _commandText = string.Empty;
        /// <summary>
        /// 参数
        /// </summary>
        object _parameters = null;
        /// <summary>
        /// 命令类型
        /// </summary>
        RdbCommandTextType _commandType = RdbCommandTextType.Text;
        /// <summary>
        /// 事务命令
        /// </summary>
        bool _transactionCommand = false;
        /// <summary>
        /// 返回值形式
        /// </summary>
        ExecuteCommandResult _commandResultType = ExecuteCommandResult.ExecuteRows;
        /// <summary>
        /// 操作对象名称
        /// </summary>
        string _objectName = string.Empty;
        /// <summary>
        /// 对象标识
        /// </summary>
        SortedSet<string> _objectKeys = null;
        /// <summary>
        /// 对象标识值
        /// </summary>
        SortedDictionary<string, dynamic> _objectKeyValues = null;
        /// <summary>
        /// 服务器对象标识
        /// </summary>
        SortedSet<string> _serverKeys = null;
        /// <summary>
        /// 服务器对象标识值
        /// </summary>
        SortedDictionary<string, dynamic> _serverKeyValues = null;
        /// <summary>
        /// 命令执行方式
        /// </summary>
        CommandExecuteMode _executeMode = CommandExecuteMode.Transform;
        /// <summary>
        /// 查询对象
        /// </summary>
        IQuery _query = null;
        /// <summary>
        /// 操作类型
        /// </summary>
        OperateType _operate = OperateType.Query;
        IEnumerable<string> _fields = null;

        #endregion

        #region propertys

        /// <summary>
        /// command text
        /// </summary>
        public string CommandText
        {
            get
            {
                return _commandText;
            }
            set
            {
                _commandText = value;
            }
        }

        /// <summary>
        /// parameters
        /// </summary>
        public object Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        /// <summary>
        /// command type
        /// </summary>
        public RdbCommandTextType CommandType
        {
            get
            {
                return _commandType;
            }
            set
            {
                _commandType = value;
            }
        }

        /// <summary>
        /// transaction command
        /// </summary>
        public bool TransactionCommand
        {
            get
            {
                return _transactionCommand;
            }
            set
            {
                _transactionCommand = value;
            }
        }

        /// <summary>
        /// result type
        /// </summary>
        public ExecuteCommandResult CommandResultType
        {
            get
            {
                return _commandResultType;
            }
            set
            {
                _commandResultType = value;
            }
        }

        /// <summary>
        /// verify result method
        /// </summary>
        public Func<int, bool> VerifyResult
        {
            get; set;
        }

        /// <summary>
        /// object name
        /// </summary>
        public string ObjectName
        {
            get
            {
                return _objectName;
            }
            set
            {
                _objectName = value;
            }
        }

        /// <summary>
        /// object keys
        /// </summary>
        public SortedSet<string> ObjectKeys
        {
            get
            {
                return _objectKeys;
            }
            set
            {
                _objectKeys = value;
            }
        }

        /// <summary>
        /// object key values
        /// </summary>
        public SortedDictionary<string, dynamic> ObjectKeyValues
        {
            get
            {
                return _objectKeyValues;
            }
            set
            {
                _objectKeyValues = value;
            }
        }

        /// <summary>
        /// server keys
        /// </summary>
        public SortedSet<string> ServerKeys
        {
            get
            {
                return _serverKeys;
            }
            set
            {
                _serverKeys = value;
            }
        }

        /// <summary>
        /// server key values
        /// </summary>
        public SortedDictionary<string, dynamic> ServerKeyValues
        {
            get
            {
                return _serverKeyValues;
            }
            set
            {
                _serverKeyValues = value;
            }
        }

        /// <summary>
        /// execute mode
        /// </summary>
        public CommandExecuteMode ExecuteMode
        {
            get
            {
                return _executeMode;
            }
            set
            {
                _executeMode = value;
            }
        }

        /// <summary>
        /// query object
        /// </summary>
        public IQuery Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value;
            }
        }

        /// <summary>
        /// operate
        /// </summary>
        public OperateType Operate
        {
            get
            {
                return _operate;
            }
            set
            {
                _operate = value;
            }
        }

        /// <summary>
        /// fields
        /// </summary>
        public IEnumerable<string> Fields
        {
            get
            {
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }

        /// <summary>
        /// success callback
        /// </summary>
        public event ExecuteCommandCallback SuccessCallback;

        /// <summary>
        /// failed callback
        /// </summary>
        public event ExecuteCommandCallback FailedCallback;

        /// <summary>
        /// callback request
        /// </summary>
        public CommandCallbackRequest CallbackRequest { get; set; }

        /// <summary>
        /// before execute
        /// </summary>
        public event BeforeExecute BeforeExecute;

        /// <summary>
        /// before request
        /// </summary>
        public BeforeExecuteRequest BeforeRequest { get; set; }

        #endregion

        #region static methods

        /// <summary>
        /// get a new rdbcommand object
        /// </summary>
        /// <param name="operate">operate</param>
        /// <param name="parameters">parameters</param>
        /// <param name="objectName">objectName</param>
        /// <param name="objectKey">objectKey</param>
        /// <returns></returns>
        public static RdbCommand CreateNewCommand(OperateType operate, object parameters = null, string objectName = "", SortedSet<string> objectKeys = null, SortedDictionary<string, dynamic> objectKeyValues = null, SortedSet<string> serverKeys = null, SortedDictionary<string, dynamic> serverKeyValues = null)
        {
            return new RdbCommand()
            {
                _operate = operate,
                _parameters = parameters,
                _objectName = objectName,
                _objectKeyValues = objectKeyValues,
                _serverKeyValues = serverKeyValues,
                _objectKeys = objectKeys,
                _serverKeys = serverKeys
            };
        }

        #endregion

        #region methods

        /// <summary>
        /// execute commplete
        /// </summary>
        /// <param name="success">success</param>
        public void ExecuteComplete(bool success)
        {
            Task.Run(() =>
            {
                if (success)
                {
                    SuccessCallback?.Invoke(CallbackRequest);
                }
                else
                {
                    FailedCallback?.Invoke(CallbackRequest);
                }
            });
        }

        /// <summary>
        /// execute before
        /// </summary>
        /// <returns></returns>
        public bool ExecuteBefore()
        {
            bool result = BeforeExecute?.Invoke(BeforeRequest) ?? true;
            return result;
        }

        #endregion
    }
}
