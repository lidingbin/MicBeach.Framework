using Dapper;
using MicBeach.Develop.Command;
using MicBeach.Develop.CQuery;
using MicBeach.Develop.CQuery.Translator;
using MicBeach.Util.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MicBeach.DB.MySQL
{
    /// <summary>
    /// imeplements dbengine for mysql
    /// </summary>
    public class MySqlEngine: IDbEngine
    {
        #region execute

        /// <summary>
        /// execute command
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">server</param>
        /// <param name="cmds">command</param>
        /// <returns>data numbers</returns>
        public int Execute(ServerInfo server, params ICommand[] cmds)
        {
            var verifyResults = VerifyCommand(cmds);
            if (verifyResults.Item2)
            {
                return ExecuteByTransaction(server, verifyResults.Item1);
            }
            return ExecuteByNoneTransaction(server, verifyResults.Item1);
        }

        /// <summary>
        /// execute by nonee transaction
        /// </summary>
        /// <param name="serverInfo">database server</param>
        /// <param name="cmds">command</param>
        /// <returns></returns>
        int ExecuteByNoneTransaction(ServerInfo serverInfo, IEnumerable<RdbCommand> cmds)
        {
            IQueryTranslator queryTranslator = QueryTranslator.GetTranslator(serverInfo);
            using (var conn = DbServerFactory.GetConnection(serverInfo))
            {
                int executeRows = 0;
                foreach (var cmd in cmds)
                {
                    executeRows += ExecuteSingleCommand(queryTranslator, conn, cmd);
                }
                return executeRows;
            }
        }

        /// <summary>
        /// execute by transaction
        /// </summary>
        /// <param name="serverInfo">database server</param>
        /// <param name="cmds">commands</param>
        /// <returns></returns>
        int ExecuteByTransaction(ServerInfo serverInfo, IEnumerable<RdbCommand> cmds)
        {
            IQueryTranslator queryTranslator = QueryTranslator.GetTranslator(serverInfo);
            using (var conn = DbServerFactory.GetConnection(serverInfo))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                bool result = true;
                int resultValue = 0;
                try
                {
                    foreach (var cmd in cmds)
                    {
                        int cmdResult = ExecuteSingleCommand(queryTranslator, conn, cmd, tran);
                        resultValue += cmdResult;
                        if (cmd.VerifyResult != null)
                        {
                            result = result && cmd.VerifyResult(cmdResult);
                        }
                    }
                    if (result)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                        resultValue = 0;
                    }
                    return resultValue;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// execute single command
        /// </summary>
        /// <param name="conn">db connection</param>
        /// <param name="cmd">command</param>
        /// <param name="tran">transaction</param>
        /// <returns></returns>
        int ExecuteSingleCommand(IQueryTranslator queryTranslator, IDbConnection conn, RdbCommand cmd, IDbTransaction tran = null)
        {
            int resultValue = 0;

            #region query object translate

            IDictionary<string, object> queryParameters = null;
            var tranResult = queryTranslator.Translate(cmd.Query);
            string conditionString = string.Empty;
            if (!tranResult.ConditionString.IsNullOrEmpty())
            {
                conditionString += "WHERE " + tranResult.ConditionString;
            }
            if (tranResult.Parameters != null)
            {
                if (tranResult.Parameters is IDictionary<string, object>)
                {
                    queryParameters = tranResult.Parameters as IDictionary<string, object>;
                }
                else
                {
                    queryParameters = tranResult.Parameters.ObjectToDcitionary();
                }
            }

            #endregion

            #region execute

            StringBuilder cmdText = new StringBuilder();
            object newParameters = cmd.Parameters;
            ExecuteCommandResult executeCmdResult = cmd.CommandResultType;
            if (cmd.ExecuteMode == CommandExecuteMode.CommandText)
            {
                cmdText.Append(cmd.CommandText);
            }
            else
            {
                switch (cmd.Operate)
                {
                    case OperateType.Insert:
                        cmdText.AppendFormat("INSERT INTO `{0}` ({1}) VALUES ({2})", cmd.ObjectName, string.Join(",", FormatFields(cmd.Fields)), string.Join(",", cmd.Fields.Select(c => "?" + c)));
                        break;
                    case OperateType.Update:
                        IDictionary<string, dynamic> oldCmdParameterDic = null;
                        IDictionary<string, dynamic> newCmdParameterDic = new Dictionary<string, dynamic>();
                        if (cmd.Parameters is IDictionary<string, dynamic>)
                        {
                            oldCmdParameterDic = cmd.Parameters as IDictionary<string, dynamic>;
                        }
                        else
                        {
                            oldCmdParameterDic = cmd.Parameters.ObjectToDcitionary();
                        }
                        List<string> updateSetArray = new List<string>();
                        foreach (var field in cmd.Fields)
                        {
                            if (oldCmdParameterDic == null || !oldCmdParameterDic.ContainsKey(field) || !(oldCmdParameterDic[field] is CalculateModify))
                            {
                                updateSetArray.Add(string.Format("{0}.`{1}`=?{2}", queryTranslator.ObjectPetName, field, field));
                                if (oldCmdParameterDic != null && oldCmdParameterDic.ContainsKey(field))
                                {
                                    newCmdParameterDic.Add(field, oldCmdParameterDic[field]);
                                }
                            }
                            else if (oldCmdParameterDic[field] is CalculateModify)
                            {
                                CalculateModify calModify = oldCmdParameterDic[field] as CalculateModify;
                                newCmdParameterDic.Add(field, calModify.Value);
                                string calChar = GetCalculateChar(calModify.Calculate);
                                updateSetArray.Add(string.Format("{0}.`{1}`={0}.`{1}`{2}?{3}", queryTranslator.ObjectPetName, field, calChar, field));
                            }
                        }
                        newParameters = newCmdParameterDic;
                        cmdText.AppendFormat("UPDATE `{2}` AS {0} SET {1} {3}", queryTranslator.ObjectPetName, string.Join(",", updateSetArray.ToArray()), cmd.ObjectName, conditionString);
                        break;
                    case OperateType.Delete:
                        cmdText.AppendFormat("DELETE {0} FROM `{1}` AS {0} {2}", queryTranslator.ObjectPetName, cmd.ObjectName, conditionString);
                        break;
                    case OperateType.Exist:
                        cmdText.AppendFormat("SELECT EXISTS(SELECT {0}.`{1}` FROM `{2}` AS {0} {3})", queryTranslator.ObjectPetName, cmd.Fields.ElementAt(0), cmd.ObjectName, conditionString);
                        executeCmdResult = ExecuteCommandResult.ExecuteScalar;
                        break;
                    default:
                        break;
                }
            }
            if (cmdText.Length <= 0)
            {
                return 0;
            }

            #endregion

            #region parameter opertion

            object parameters = queryParameters;
            if (newParameters != null)
            {
                CmdParameters cmdParameters = newParameters as CmdParameters;
                if (cmdParameters != null)
                {
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    foreach (var item in cmdParameters.Parameters)
                    {
                        dynamicParameters.Add(item.Key, item.Value.Value, item.Value.DbType, item.Value.ParameterDirection, item.Value.Size, item.Value.Precision, item.Value.Scale);
                    }
                    if (queryParameters != null)
                    {
                        foreach (var parameter in queryParameters)
                        {
                            dynamicParameters.Add(parameter.Key, parameter.Value);
                        }
                    }
                    parameters = dynamicParameters;
                }
                else if (newParameters is IDictionary<string, dynamic>)
                {
                    IDictionary<string, dynamic> cmdParametersDic = newParameters as IDictionary<string, dynamic>;
                    if (queryParameters != null)
                    {
                        foreach (var queryParaItem in queryParameters)
                        {
                            cmdParametersDic.Add(queryParaItem.Key, queryParaItem.Value);
                        }
                    }
                    parameters = cmdParametersDic;
                }
                else
                {
                    IDictionary<string, object> cmdParametersDic = newParameters.ObjectToDcitionary();
                    if (queryParameters != null)
                    {
                        foreach (var queryParaItem in queryParameters)
                        {
                            cmdParametersDic.Add(queryParaItem.Key, queryParaItem.Value);
                        }
                    }
                    parameters = cmdParametersDic;
                }
            }

            #endregion

            switch (executeCmdResult)
            {
                case ExecuteCommandResult.ExecuteScalar:
                    resultValue = conn.ExecuteScalar<int>(cmdText.ToString(), parameters, transaction: tran, commandType: GetCommandType(cmd));
                    break;
                case ExecuteCommandResult.ExecuteRows:
                    resultValue = conn.Execute(cmdText.ToString(), parameters, transaction: tran, commandType: GetCommandType(cmd));
                    break;
            }
            return resultValue;
        }

        #endregion

        #region query

        /// <summary>
        /// query data list
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">database server</param>
        /// <param name="cmd">command</param>
        /// <returns>data list</returns>
        public IEnumerable<T> Query<T>(ServerInfo server, ICommand cmd)
        {
            var verifyResults = VerifyCommand(cmd);
            cmd.Query = cmd.Query ?? QueryFactory.Create();

            #region query object translate

            IQueryTranslator queryTranslator = QueryTranslator.GetTranslator(server);
            var tranResult = queryTranslator.Translate(cmd.Query);

            #endregion

            #region execute

            StringBuilder cmdText = new StringBuilder();
            switch (cmd.Query.QueryType)
            {
                case QueryCommandType.Text:
                    cmdText.Append(tranResult.ConditionString);
                    break;
                case QueryCommandType.QueryObject:
                default:
                    int size = cmd.Query == null ? 0 : cmd.Query.QuerySize;
                    string fieldSplitChar = string.Format(",{0}.", queryTranslator.ObjectPetName);
                    cmdText.AppendFormat("SELECT {0}.{1} FROM `{2}` AS {0}", queryTranslator.ObjectPetName, string.Join(fieldSplitChar, FormatFields(cmd.Fields)), cmd.ObjectName);
                    if (!tranResult.ConditionString.IsNullOrEmpty())
                    {
                        cmdText.AppendFormat(" WHERE {0}", tranResult.ConditionString);
                    }
                    if (!tranResult.OrderString.IsNullOrEmpty())
                    {
                        cmdText.AppendFormat(" ORDER BY {0}", tranResult.OrderString);
                    }
                    if (size > 0)
                    {
                        cmdText.AppendFormat(" LIMIT 0,{0}", size);
                    }
                    break;
            }

            #endregion

            using (var conn = DbServerFactory.GetConnection(server))
            {
                return conn.Query<T>(cmdText.ToString(), tranResult.Parameters, commandType: GetCommandType(verifyResults.Item1.First()));
            }
        }

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">databse server</param>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        public IEnumerable<T> QueryPaging<T>(ServerInfo server, ICommand cmd)
        {
            int beginIndex = 0;
            int pageSize = 1;
            if (cmd.Query != null && cmd.Query.PagingInfo != null)
            {
                beginIndex = cmd.Query.PagingInfo.Page;
                pageSize = cmd.Query.PagingInfo.PageSize;
                beginIndex = (beginIndex - 1) * pageSize;
            }
            return QueryOffset<T>(server, cmd, beginIndex, pageSize);
        }

        /// <summary>
        /// query data list offset the specified numbers
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">database server</param>
        /// <param name="cmd">command</param>
        /// <param name="offsetNum">offset num</param>
        /// <param name="size">query size</param>
        /// <returns></returns>
        public IEnumerable<T> QueryOffset<T>(ServerInfo server, ICommand cmd, int offsetNum = 0, int size = int.MaxValue)
        {
            var verifyResults = VerifyCommand(cmd);
            cmd.Query = cmd.Query ?? QueryFactory.Create();

            #region query object translate

            IQueryTranslator queryTranslator = QueryTranslator.GetTranslator(server);
            var tranResult = queryTranslator.Translate(cmd.Query);

            #endregion

            #region execute

            StringBuilder cmdText = new StringBuilder();
            switch (cmd.Query.QueryType)
            {
                case QueryCommandType.Text:
                    cmdText.Append(tranResult.ConditionString);
                    break;
                case QueryCommandType.QueryObject:
                default:
                    string fieldSplitChar = string.Format(",{0}.", queryTranslator.ObjectPetName);
                    string conditionString = string.Empty;
                    string orderString = string.Empty;
                    string limitString = string.Empty;
                    if (!tranResult.ConditionString.IsNullOrEmpty())
                    {
                        conditionString = string.Format(" WHERE {0}", tranResult.ConditionString);
                    }
                    if (!tranResult.OrderString.IsNullOrEmpty())
                    {
                        orderString = string.Format(" ORDER BY {0}", tranResult.OrderString);
                    }
                    limitString = string.Format(" LIMIT {0},{1}", offsetNum, size);
                    cmdText.AppendFormat("SELECT (SELECT COUNT({3}.`{0}`) FROM `{2}` AS {3} {4}) AS PagingTotalCount,{3}.{1} FROM `{2}` AS {3} {4} {5} {6}", cmd.Fields.ElementAt(0), string.Join(fieldSplitChar, FormatFields(cmd.Fields)), cmd.ObjectName, queryTranslator.ObjectPetName, conditionString, orderString, limitString);
                    break;
            }

            #endregion

            using (var conn = DbServerFactory.GetConnection(server))
            {
                return conn.Query<T>(cmdText.ToString(), tranResult.Parameters, commandType: GetCommandType(verifyResults.Item1.First()));
            }
        }

        /// <summary>
        /// determine whether data has existed
        /// </summary>
        /// <param name="server">server</param>
        /// <param name="cmd">command</param>
        /// <returns>data has existed</returns>
        public bool Query(ServerInfo server, ICommand cmd)
        {
            int value = Execute(server, cmd);
            return value > 0;
        }

        /// <summary>
        /// query single value
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">database server</param>
        /// <param name="cmd">command</param>
        /// <returns>query data</returns>
        public T QuerySingle<T>(ServerInfo server, ICommand cmd)
        {
            T result = default(T);
            switch (cmd.Operate)
            {
                case OperateType.Max:
                case OperateType.Min:
                case OperateType.Sum:
                case OperateType.Avg:
                case OperateType.Count:
                    result = AggregateFunction<T>(server, cmd);
                    break;
                case OperateType.Query:
                    if (cmd.Query == null)
                    {
                        cmd.Query = QueryFactory.Create();
                    }
                    cmd.Query.QuerySize = 1;
                    IEnumerable<T> dataList = Query<T>(server, cmd);
                    if (dataList != null && dataList.Any())
                    {
                        result = dataList.ElementAt(0);
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Aggregate Function
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="server">database server</param>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        T AggregateFunction<T>(ServerInfo server, ICommand cmd)
        {
            var verifyResults = VerifyCommand(cmd);
            cmd.Query = cmd.Query ?? QueryFactory.Create();

            #region query object translate

            IQueryTranslator queryTranslator = QueryTranslator.GetTranslator(server);
            var tranResult = queryTranslator.Translate(cmd.Query);

            #endregion

            #region query

            StringBuilder cmdText = new StringBuilder();
            switch (cmd.Query.QueryType)
            {
                case QueryCommandType.Text:
                    cmdText.Append(tranResult.ConditionString);
                    break;
                case QueryCommandType.QueryObject:
                default:
                    string funcName = GetAggregateFunctionName(cmd.Operate);
                    if (funcName.IsNullOrEmpty() || cmd.Fields.IsNullOrEmpty())
                    {
                        return default(T);
                    }
                    cmdText.AppendFormat("SELECT {0}({3}.`{1}`) FROM `{2}` AS {3}", funcName, cmd.Fields.ElementAt(0), cmd.ObjectName, queryTranslator.ObjectPetName);
                    if (!tranResult.ConditionString.IsNullOrEmpty())
                    {
                        cmdText.AppendFormat(" WHERE {0}", tranResult.ConditionString);
                    }
                    if (!tranResult.OrderString.IsNullOrEmpty())
                    {
                        cmdText.AppendFormat(" ORDER BY {0}", tranResult.OrderString);
                    }
                    break;
            }

            #endregion

            using (var conn = DbServerFactory.GetConnection(server))
            {
                return conn.ExecuteScalar<T>(cmdText.ToString(), tranResult.Parameters, commandType: GetCommandType(verifyResults.Item1.First()));
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// verify command
        /// </summary>
        /// <param name="cmds"></param>
        Tuple<IEnumerable<RdbCommand>, bool> VerifyCommand(params ICommand[] cmds)
        {
            if (cmds == null || !cmds.Any())
            {
                throw new Exception("At Least Special One Execute Command");
            }
            bool transaction = false;
            List<RdbCommand> rdbCommandList = new List<RdbCommand>();
            foreach (var cmd in cmds)
            {
                if (cmd == null)
                {
                    throw new Exception("Null Command");
                }
                RdbCommand rdbCmd = cmd as RdbCommand;
                if (rdbCmd == null)
                {
                    throw new Exception("Command Is Not RDB Command");
                }
                transaction = transaction && rdbCmd.TransactionCommand;
                rdbCommandList.Add(rdbCmd);
            }
            transaction = transaction || rdbCommandList.Count > 1;
            return new Tuple<IEnumerable<RdbCommand>, bool>(rdbCommandList, transaction);
        }

        /// <summary>
        /// get command type
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        CommandType GetCommandType(RdbCommand cmd)
        {
            return cmd.CommandType == RdbCommandTextType.Procedure ? CommandType.StoredProcedure : CommandType.Text;
        }

        /// <summary>
        /// get calculate sign
        /// </summary>
        /// <param name="calculate">calculate operator</param>
        /// <returns></returns>
        string GetCalculateChar(CalculateOperator calculate)
        {
            string opearterChar = "";
            switch (calculate)
            {
                case CalculateOperator.Add:
                    opearterChar = "+";
                    break;
                case CalculateOperator.subtract:
                    opearterChar = "-";
                    break;
                case CalculateOperator.multiply:
                    opearterChar = "*";
                    break;
                case CalculateOperator.divide:
                    opearterChar = "/";
                    break;
            }
            return opearterChar;
        }

        /// <summary>
        /// get aggregate function name
        /// </summary>
        /// <param name="funcType">function type</param>
        /// <returns></returns>
        string GetAggregateFunctionName(OperateType funcType)
        {
            string funcName = string.Empty;
            switch (funcType)
            {
                case OperateType.Max:
                    funcName = "MAX";
                    break;
                case OperateType.Min:
                    funcName = "MIN";
                    break;
                case OperateType.Sum:
                    funcName = "SUM";
                    break;
                case OperateType.Avg:
                    funcName = "AVG";
                    break;
                case OperateType.Count:
                    funcName = "COUNT";
                    break;
            }
            return funcName;
        }

        /// <summary>
        /// format fields
        /// </summary>
        /// <param name="fields">fields</param>
        /// <returns></returns>
        IEnumerable<string> FormatFields(IEnumerable<string> fields)
        {
            if (fields.IsNullOrEmpty())
            {
                return new List<string>(0);
            }
            return fields.Select(c => { return string.Format("`{0}`", c); });
        }

        #endregion
    }
}
