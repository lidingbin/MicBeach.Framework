
using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Repository
{
    /// <summary>
    /// 服务节点执行计划存储
    /// </summary>
    public interface ITriggerServerRepository : IRepository<TriggerServer>
    {
        #region 根据执行计划移除计划服务

        /// <summary>
        /// 根据执行计划移除计划服务
        /// </summary>
        /// <param name="triggers">执行计划数据</param>
        void RemoveTriggerServerFromTrigger(IEnumerable<Trigger> triggers);

        #endregion

        #region 根据服务节点移除计划服务

        /// <summary>
        /// 根据服务节点移除计划服务
        /// </summary>
        /// <param name="servers">服务数据</param>
        void RemoveTriggerServerFromServer(IEnumerable<ServerNode> servers);

        #endregion

        #region 移除任务&服务承载时移除服务执行计划

        /// <summary>
        /// 移除任务&服务承载时移除服务执行计划
        /// </summary>
        /// <param name="jobServerHosts">任务服务承载信息</param>
        void RemoveTriggerServerFromJobHost(IEnumerable<JobServerHost> jobServerHosts);

        #endregion
    }
}
