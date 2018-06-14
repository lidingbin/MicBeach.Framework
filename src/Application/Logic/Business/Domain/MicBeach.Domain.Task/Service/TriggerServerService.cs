using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Domain.Task.Model;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.IoC;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Util;
using MicBeach.Util.Paging;
using MicBeach.Util.CustomerException;

namespace MicBeach.Domain.Task.Service
{
    /// <summary>
    /// 计划服务操作
    /// </summary>
    public static class TriggerServerService
    {
        static ITriggerServerRepository triggerServerRepository = ContainerManager.Resolve<ITriggerServerRepository>();

        #region 删除执行计划服务节点

        /// <summary>
        /// 删除执行计划服务节点
        /// </summary>
        /// <param name="triggerServers">节点服务信息</param>
        public static void DeleteTriggerServer(IEnumerable<TriggerServer> triggerServers)
        {
            if (triggerServers.IsNullOrEmpty())
            {
                return;
            }
            triggerServerRepository.Remove(triggerServers.ToArray());
        }

        #endregion

        #region 修改运行状态

        /// <summary>
        /// 修改运行状态
        /// </summary>
        /// <param name="triggerServers">节点服务信息</param>
        public static void ModifyRunState(IEnumerable<TriggerServer> triggerServers)
        {
            if (triggerServers.IsNullOrEmpty())
            {
                return;
            }
            IQuery query = QueryFactory.Create<TriggerServerQuery>();
            foreach (var triggerServer in triggerServers)
            {
                if (triggerServer == null || triggerServer.Server == null || triggerServer.Trigger == null)
                {
                    continue;
                }
                query.Or<TriggerServerQuery>(c => c.Trigger == triggerServer.Trigger.Id && c.Server == triggerServer.Server.Id);
            }
            List<TriggerServer> triggerServerList = triggerServerRepository.GetList(query);
            var triggerServerCompare = new TriggerServerCompare();
            if (!triggerServers.Except(triggerServerList, triggerServerCompare).IsNullOrEmpty())
            {
                throw new AppException("请指定正确的操作信息");
            }
            triggerServerList.ForEach(c =>
            {
                var newStateObj = triggerServers.FirstOrDefault(r => triggerServerCompare.Equals(c, r));
                if (newStateObj != null)
                {
                    c.RunState = newStateObj.RunState;
                }
            });
            triggerServerRepository.Save(triggerServerList.ToArray());
        }

        #endregion

        #region 保存计划服务承载信息

        /// <summary>
        /// 保存计划服务承载信息
        /// </summary>
        /// <param name="newTriggerServers">计划服务信息</param>
        public static void SaveTriggerServer(IEnumerable<TriggerServer> newTriggerServers)
        {
            if (newTriggerServers.IsNullOrEmpty())
            {
                return;
            }
            IQuery query = QueryFactory.Create<TriggerServerQuery>();
            foreach (var triggerServer in newTriggerServers)
            {
                if (triggerServer == null || triggerServer.Server == null || triggerServer.Trigger == null)
                {
                    continue;
                }
                query.Or<TriggerServerQuery>(c => c.Trigger == triggerServer.Trigger.Id && c.Server == triggerServer.Server.Id);
            }
            List<TriggerServer> triggerServerList = triggerServerRepository.GetList(query);
            newTriggerServers = newTriggerServers.Except(triggerServerList, new TriggerServerCompare());//移除当前已经存在的
            if (newTriggerServers.IsNullOrEmpty())
            {
                return;
            }
            foreach (var newTriggerServer in newTriggerServers)
            {
                newTriggerServer.Save();
            }
        }

        #endregion

        #region 获取服务节点执行计划

        /// <summary>
        /// 获取服务节点执行计划
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static TriggerServer GetTriggerServer(IQuery query)
        {
            var triggerServer = triggerServerRepository.Get(query);
            return triggerServer;
        }

        #endregion

        #region 获取服务节点执行计划列表

        /// <summary>
        /// 获取服务节点执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public static List<TriggerServer> GetTriggerServerList(IQuery query)
        {
            var triggerServerList = triggerServerRepository.GetList(query);
            return triggerServerList;
        }

        #endregion

        #region 获取服务节点执行计划分页

        /// <summary>
        /// 获取服务节点执行计划分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static IPaging<TriggerServer> GetTriggerServerPaging(IQuery query)
        {
            var triggerServerPaging = triggerServerRepository.GetPaging(query);
            return triggerServerPaging;
        }

        #endregion
    }
}
