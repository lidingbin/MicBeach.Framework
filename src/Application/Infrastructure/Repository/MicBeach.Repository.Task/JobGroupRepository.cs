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
using MicBeach.Domain.Task.Repository;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 工作分组存储
    /// </summary>
    public class JobGroupRepository : DefaultRepository<JobGroup, JobGroupEntity, IJobGroupDataAccess>,IJobGroupRepository
    {
        //事件绑定
        protected override void BindEvent()
        {
            base.BindEvent();
            //任务信息
            IJobRepository jobRepository = this.Instance<IJobRepository>();
            RemoveEvent += jobRepository.RemoveJobFromJobGroup;
        }
    }
}
