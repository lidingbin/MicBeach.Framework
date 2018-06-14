using MicBeach.Develop.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Paging;
using MicBeach.Util.Extension;
using System.Linq.Expressions;
using System.Reflection;

namespace MicBeach.DB
{
    /// <summary>
    /// db command engine
    /// </summary>
    public class DBCommandEngine : ICommandEngine
    {
        static DBCommandEngine()
        {
        }

        #region execute

        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="cmds">commands</param>
        /// <returns>date numbers </returns>
        public int Execute(params ICommand[] cmds)
        {
            if (cmds.IsNullOrEmpty())
            {
                return 0;
            }
            Dictionary<string, List<ICommand>> commandGroup = new Dictionary<string, List<ICommand>>();
            Dictionary<string, ServerInfo> serverInfos = new Dictionary<string, ServerInfo>();

            #region get database servers

            foreach (var cmd in cmds)
            {
                var servers = GetServer(cmd);
                foreach (var server in servers)
                {
                    string serverKey = server.Key;
                    if (serverInfos.ContainsKey(serverKey))
                    {
                        commandGroup[serverKey].Add(cmd);
                    }
                    else
                    {
                        commandGroup.Add(serverKey, new List<ICommand>() { cmd });
                        serverInfos.Add(serverKey, server);
                    }
                }
            }

            #endregion

            #region verify database server engine

            IEnumerable<ServerType> serverTypeList = serverInfos.Values.Select(c => c.ServerType).Distinct();
            VerifyServerEngine(serverTypeList.ToArray());

            #endregion

            #region execute commands

            int totalVal = 0;
            foreach (var cmdGroup in commandGroup)
            {
                ServerInfo serverInfo = serverInfos[cmdGroup.Key];
                IDbEngine engine = DBConfig.DbEngines[serverInfo.ServerType];
                totalVal += engine.Execute(serverInfo, cmdGroup.Value.ToArray());
            }
            return totalVal;

            #endregion
        }

        #endregion

        #region query datas

        /// <summary>
        /// determine whether data is exist
        /// </summary>
        /// <param name="cmd">command</param>
        /// <returns>data is exist</returns>
        public bool Query(ICommand cmd)
        {
            var servers = GetServer(cmd);
            VerifyServerEngine(servers.Select(c => c.ServerType).ToArray());
            bool result = false;
            foreach (var server in servers)
            {
                var engine = DBConfig.DbEngines[server.ServerType];
                result = result || engine.Query(server, cmd);//只要在一台服务器上查到数据就说明有数据
            }
            return result;
        }

        /// <summary>
        /// execute query
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>datas</returns>
        public IEnumerable<T> Query<T>(ICommand cmd)
        {
            var servers = GetServer(cmd);
            VerifyServerEngine(servers.Select(c => c.ServerType).ToArray());
            IEnumerable<T> dataList = null;
            if (servers.Count == 1)
            {
                var nowServer = servers[0];
                var engine = DBConfig.DbEngines[nowServer.ServerType];
                dataList = engine.Query<T>(nowServer, cmd);
            }
            else
            {
                foreach (var server in servers)
                {
                    var engine = DBConfig.DbEngines[server.ServerType];
                    var newDataList = engine.Query<T>(server, cmd);
                    dataList = dataList == null ? newDataList : dataList.Union(newDataList);//合并从所有服务器返回的数据
                }
                dataList = DistinctAndOrder(dataList, cmd);//去重和排序
                //指定返回数量
                if (cmd.Query != null && cmd.Query.QuerySize > 0)
                {
                    dataList = dataList.Take(cmd.Query.QuerySize);
                }
            }
            return dataList;
        }

        /// <summary>
        /// query data with paging
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        public IPaging<T> QueryPaging<T>(ICommand cmd) where T : CommandEntity<T>
        {
            var servers = GetServer(cmd);
            VerifyServerEngine(servers.Select(c => c.ServerType).ToArray());
            var server = servers[0];
            return SingleServerPaging<T>(servers[0], cmd);
        }

        /// <summary>
        /// query paging with single server
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        IPaging<T> SingleServerPaging<T>(ServerInfo server, ICommand cmd) where T : CommandEntity<T>
        {
            var engine = DBConfig.DbEngines[server.ServerType];
            IEnumerable<T> dataList = engine.QueryPaging<T>(server, cmd);
            if (dataList.IsNullOrEmpty())
            {
                return new Paging<T>(1, 0, 0, dataList);
            }
            int totalCount = dataList.ElementAt(0).GetPagingTotalCount();
            int page = 1;
            int pageSize = 1;
            if (cmd.Query != null && cmd.Query.PagingInfo != null)
            {
                page = cmd.Query.PagingInfo.Page;
                pageSize = cmd.Query.PagingInfo.PageSize;
            }
            Paging<T> dataPaging = new Paging<T>(page, pageSize, totalCount, dataList);
            return dataPaging;
        }

        ///// <summary>
        ///// 多台服务器分页
        ///// </summary>
        ///// <param name="servers">服务器方法</param>
        ///// <param name="cmd">查询命令</param>
        ///// <returns></returns>
        //IPaging<T> MultipleServerPaging<T>(List<ServerInfo> servers, ICommand cmd)
        //{
        //    //2次查询分页方法
        //    IEnumerable<T> dataList = null;
        //    int page = 1;
        //    int pageSize = 1;
        //    if (cmd.Query != null && cmd.Query.PagingInfo != null)
        //    {
        //        page = cmd.Query.PagingInfo.Page;
        //        pageSize = cmd.Query.PagingInfo.PageSize;
        //    }
        //    int offsetNum = (page - 1) * pageSize;//要跳过的数量
        //    int serverCount = servers.Count;//服务器数量
        //    int everyServerOffsetNum = offsetNum / serverCount;//每台服务器要跳过的数据
        //    Dictionary<string, IEnumerable<T>> firstDatas = new Dictionary<string, IEnumerable<T>>();//第一次数据
        //    foreach (var server in servers)
        //    {
        //        var engine = DBConfig.DbEngines[server.ServerType];
        //        firstDatas.Add(server.Key, engine.QueryOffset<T>(server, cmd, everyServerOffsetNum, pageSize));
        //    }
        //}

        /// <summary>
        /// query a single data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>data</returns>
        public T QuerySingle<T>(ICommand cmd)
        {
            T result;
            switch (cmd.Operate)
            {
                case OperateType.Max:
                case OperateType.Min:
                case OperateType.Sum:
                case OperateType.Avg:
                case OperateType.Count:
                    result = AggregateFunction<T>(cmd);
                    break;
                case OperateType.Query:
                    result = QuerySingleObject<T>(cmd);
                    break;
                default:
                    result = default(T);
                    break;
            }
            return result;
        }

        /// <summary>
        /// query a single data
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        T QuerySingleObject<T>(ICommand cmd)
        {
            var servers = GetServer(cmd);
            VerifyServerEngine(servers.Select(c => c.ServerType).ToArray());
            T result = default(T);
            foreach (var server in servers)
            {
                var engine = DBConfig.DbEngines[server.ServerType];
                var nowData = engine.QuerySingle<T>(server, cmd);
                if (nowData != null)
                {
                    result = nowData;
                    break;//只要找到值就不继续查询
                }
            }
            return result;
        }

        /// <summary>
        /// Aggregate Function
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="cmd">command</param>
        /// <returns>query data</returns>
        T AggregateFunction<T>(ICommand cmd)
        {
            var servers = GetServer(cmd);
            VerifyServerEngine(servers.Select(c => c.ServerType).ToArray());
            List<T> datas = new List<T>(servers.Count);
            foreach (var server in servers)
            {
                var engine = DBConfig.DbEngines[server.ServerType];
                datas.Add(engine.QuerySingle<T>(server, cmd));
            }
            if (datas.Count == 1)
            {
                return datas[0];
            }
            dynamic result = default(T);
            switch (cmd.Operate)
            {
                case OperateType.Max:
                    result = datas.Max();
                    break;
                case OperateType.Min:
                    result = datas.Min();
                    break;
                case OperateType.Sum:
                case OperateType.Count:
                    result = Sum(datas);
                    break;
                case OperateType.Avg:
                    result = Average(datas);
                    break;
            }
            return result;
        }

        #endregion

        #region Helper

        /// <summary>
        /// get servers
        /// </summary>
        /// <param name="command">command</param>
        /// <returns>server list</returns>
        static List<ServerInfo> GetServer(ICommand command)
        {
            if (command == null || DBConfig.GetDBServerMethod == null)
            {
                return null;
            }
            var servers = DBConfig.GetDBServerMethod(command);
            if (servers.IsNullOrEmpty())
            {
                throw new Exception("any ICommand cann't get server");
            }
            return servers.ToList();
        }

        /// <summary>
        /// verify server engine
        /// </summary>
        /// <param name="serverTypes">server types</param>
        void VerifyServerEngine(params ServerType[] serverTypes)
        {
            if (serverTypes == null)
            {
                return;
            }
            if (DBConfig.DbEngines == null || DBConfig.DbEngines.Count <= 0)
            {
                throw new Exception("not config any IDbEngine Data");
            }
            foreach (var serverType in serverTypes)
            {
                if (!DBConfig.DbEngines.ContainsKey(serverType) || DBConfig.DbEngines[serverType] == null)
                {
                    throw new Exception(string.Format("ServerType:{0} not special execute engine", serverType.ToString()));
                }
            }
        }

        /// <summary>
        /// calculate sum
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="datas">data list</param>
        /// <returns></returns>
        dynamic Sum<T>(IEnumerable<T> datas)
        {
            dynamic result = default(T);
            foreach (dynamic data in datas)
            {
                result += data;
            }
            return result;
        }

        /// <summary>
        /// calculate average
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="datas">data list</param>
        /// <returns></returns>
        dynamic Average<T>(IEnumerable<T> datas)
        {
            dynamic result = default(T);
            int count = 0;
            foreach (dynamic data in datas)
            {
                result += data;
                count++;
            }
            return result / count;
        }

        /// <summary>
        /// distinct and sort
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="datas">datas</param>
        /// <param name="cmd">command</param>
        /// <returns></returns>
        IEnumerable<T> DistinctAndOrder<T>(IEnumerable<T> datas, ICommand cmd)
        {
            if (datas == null || !datas.Any())
            {
                return datas;
            }
            datas = datas.Distinct(new EntityCompare<T>());
            //order data
            if (cmd.Query != null)
            {
                datas = cmd.Query.Order(datas);
            }
            return datas;
        }

        #endregion
    }
}
