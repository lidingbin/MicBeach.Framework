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
    /// 工作任务存储
    /// </summary>
    public interface IJobRepository : IRepository<Job>
    {
        #region 移除工作日分组时移除任务信息

        /// <summary>
        /// 移除工作分组时移除任务信息
        /// </summary>
        /// <param name="jobGroups">任务分组</param>
        void RemoveJobFromJobGroup(IEnumerable<JobGroup> jobGroups);

        #endregion
    }
}
