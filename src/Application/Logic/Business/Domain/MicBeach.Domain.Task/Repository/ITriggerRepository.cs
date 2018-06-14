
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
    /// 任务执行计划存储
    /// </summary>
    public interface ITriggerRepository : IRepository<Trigger>
    {
        #region 删除工作任务时删除相应的计划

        /// <summary>
        /// 删除工作任务时删除相应的计划
        /// </summary>
        /// <param name="jobs">工作任务</param>
        void RemoveTriggerFromJob(IEnumerable<Job> jobs);

        #endregion
    }
}
