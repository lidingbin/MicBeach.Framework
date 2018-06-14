using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Domain.Task.Model;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Entity.Task;
using MicBeach.Util.Extension;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.Query.Task;
using MicBeach.Domain.Task.Repository;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 工作承载节点存储
    /// </summary>
    public class JobServerHostRepository : DefaultRepository<JobServerHost, JobServerHostEntity, IJobServerHostDataAccess>, IJobServerHostRepository
    {
        #region 删除工作时删除工作承载信息

        /// <summary>
        ///  删除工作时删除工作承载信息
        /// </summary>
        /// <param name="jobs">工作信息</param>
        public void RemoveJobServerHostByJob(IEnumerable<Job> jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<string> jobIds = jobs.Select(c => c.Id).Distinct();
            IQuery query = QueryFactory.Create<JobServerHostQuery>(c => jobIds.Contains(c.Job));
            UnitOfWork.RegisterCommand(dataAccess.Delete(query));
        }

        #endregion

        #region 删除服务节点时删除工作承载信息

        /// <summary>
        /// 删除服务节点时删除工作承载信息
        /// </summary>
        /// <param name="servers">服务信息</param>
        public void RemoveJobServerHostByServer(IEnumerable<ServerNode> servers)
        {
            if (servers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<string> serverIds = servers.Select(c => c.Id).Distinct();
            IQuery query = QueryFactory.Create<JobServerHostQuery>(c => serverIds.Contains(c.Server));
            UnitOfWork.RegisterCommand(dataAccess.Delete(query));
        }

        #endregion

        #region 绑定事件

        protected override void BindEvent()
        {
            base.BindEvent();
            //移除服务承载时移除服务下的执行计划
            ITriggerServerRepository triggerServerRepository = this.Instance<ITriggerServerRepository>();
            RemoveEvent += triggerServerRepository.RemoveTriggerServerFromJobHost;
        }

        #endregion
    }
}
