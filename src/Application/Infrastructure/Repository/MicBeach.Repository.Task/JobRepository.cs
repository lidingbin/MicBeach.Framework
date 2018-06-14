using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using MicBeach.Entity.Task;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Domain.Task.Repository;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 工作任务存储
    /// </summary>
    public class JobRepository : DefaultRepository<Job, JobEntity, IJobDataAccess>, IJobRepository
    {
        /// <summary>
        /// 事件绑定
        /// </summary>
        protected override void BindEvent()
        {
            base.BindEvent();
            //任务服务
            IJobServerHostRepository jobServerHostRepository = this.Instance<IJobServerHostRepository>();
            RemoveEvent += jobServerHostRepository.RemoveJobServerHostByJob;//删除对应的工作承载信息
            //任务计划
            ITriggerRepository triggerRepository = this.Instance<ITriggerRepository>();
            RemoveEvent += triggerRepository.RemoveTriggerFromJob;
        }

        #region 移除工作日分组时移除任务信息

        /// <summary>
        /// 移除工作分组时移除任务信息
        /// </summary>
        /// <param name="jobGroups">任务分组</param>
        public void RemoveJobFromJobGroup(IEnumerable<JobGroup> jobGroups)
        {
            if (jobGroups.IsNullOrEmpty())
            {
                return;
            }
            List<string> groupIds = jobGroups.Select(c => c.Code).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<JobQuery>(c => groupIds.Contains(c.Group));
            Remove(removeQuery);
        }

        #endregion
    }
}
