using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.IoC;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Util.Paging;
using MicBeach.Util;
using MicBeach.Util.CustomerException;

namespace MicBeach.Domain.Task.Service
{
    /// <summary>
    /// 服务节点服务
    /// </summary>
    public static class ServerNodeService
    {
        static IServerNodeRepository serverNodeRepository = ContainerManager.Resolve<IServerNodeRepository>();

        #region 删除服务节点

        /// <summary>
        /// 删除服务节点
        /// </summary>
        /// <param name="servers">服务信息</param>
        public static void DeleteServerNode(IEnumerable<ServerNode> servers)
        {
            if (servers.IsNullOrEmpty())
            {
                return;
            }
            serverNodeRepository.Remove(servers.ToArray());
        }

        /// <summary>
        /// 删除服务节点
        /// </summary>
        /// <param name="serverCodes">服务编码</param>
        public static void DeleteServerNode(IEnumerable<string> serverCodes)
        {
            if (serverCodes.IsNullOrEmpty())
            {
                return;
            }
            DeleteServerNode(serverCodes.Select(c => ServerNode.CreateServerNode(c)));
        }

        #endregion

        #region 获取服务节点

        /// <summary>
        /// 获取服务节点
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>服务节点</returns>
        public static ServerNode GetServerNode(IQuery query)
        {
            return serverNodeRepository.Get(query);
        }

        /// <summary>
        /// 根据编号获取服务节点
        /// </summary>
        /// <param name="id">服务编号</param>
        /// <returns>服务节点</returns>
        public static ServerNode GetServerNode(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return null;
            }
            IQuery query = QueryFactory.Create<ServerNodeQuery>(c => c.Id == id);
            return GetServerNode(query);
        }

        #endregion

        #region 获取服务节点列表

        /// <summary>
        /// 获取服务节点列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>服务节点列表</returns>
        public static List<ServerNode> GetServerNodeList(IQuery query)
        {
            return serverNodeRepository.GetList(query);
        }

        /// <summary>
        /// 获取服务节点列表
        /// </summary>
        /// <param name="ids">节点列表</param>
        /// <returns>节点列表</returns>
        public static List<ServerNode> GetServerNodeList(IEnumerable<string> ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return new List<ServerNode>(0);
            }
            IQuery query = QueryFactory.Create<ServerNodeQuery>(c => ids.Contains(c.Id));
            return GetServerNodeList(query);
        }

        #endregion

        #region 获取服务节点分页

        /// <summary>
        /// 获取服务节点分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static IPaging<ServerNode> GetServerNodePaging(IQuery query)
        {
            return serverNodeRepository.GetPaging(query);
        }

        #endregion

        #region 修改服务节点运行状态

        /// <summary>
        /// 修改服务节点运行状态
        /// </summary>
        /// <param name="servers">要操作的服务信息</param>
        public static void ModifyServerNodeRunState(IEnumerable<ServerNode> servers)
        {
            if (servers.IsNullOrEmpty())
            {
                return;
            }
            var serverIds = servers.Select(c => c.Id);
            var nowServers = serverNodeRepository.GetList(QueryFactory.Create<ServerNodeQuery>(c => serverIds.Contains(c.Id)));
            if (nowServers.IsNullOrEmpty())
            {
                return;
            }
            foreach (var nowServer in nowServers)
            {
                var newServer = servers.FirstOrDefault(c => c.Id == nowServer.Id);
                if (newServer == null)
                {
                    continue;
                }
                nowServer.State = newServer.State;
                nowServer.Save();
            }
        }

        #endregion

        #region 保存服务节点

        /// <summary>
        /// 保存服务节点
        /// </summary>
        /// <param name="server">服务节点对象</param>
        /// <returns>执行结果</returns>
        public static void SaveServerNode(ServerNode server)
        {
            if (server == null)
            {
                return;
            }
            if (server.Id.IsNullOrEmpty())
            {
                AddServerNode(server);
            }
            else
            {
                UpdateServerNode(server);
            }
        }

        /// <summary>
        /// 添加服务节点
        /// </summary>
        /// <param name="server">服务节点对象</param>
        /// <returns>执行结果</returns>
        static void AddServerNode(ServerNode server)
        {
            server.Save();
        }

        /// <summary>
        /// 更新服务节点
        /// </summary>
        /// <param name="server">服务节点对象</param>
        /// <returns>执行结果</returns>
        static void UpdateServerNode(ServerNode server)
        {
            var nowServerNode = serverNodeRepository.Get(QueryFactory.Create<ServerNodeQuery>(c => c.Id == server.Id));
            if (nowServerNode == null)
            {
                throw new AppException("请指定要操作的服务信息");
            }
            nowServerNode.Name = server.Name;
            nowServerNode.InstanceName = server.InstanceName;
            nowServerNode.State = server.State;
            nowServerNode.ThreadCount = server.ThreadCount;
            nowServerNode.ThreadPriority = server.ThreadPriority;
            nowServerNode.Description = server.Description;
            nowServerNode.Host = server.Host;
            nowServerNode.Save();
        }

        #endregion
    }
}
