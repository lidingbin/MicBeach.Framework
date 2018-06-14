using MicBeach.ServiceInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util;
using MicBeach.BusinessInterface.Task;
using MicBeach.DTO.Task.Query;
using MicBeach.Util.Response;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util.Paging;

namespace MicBeach.Service.Task
{
    /// <summary>
    /// 工作任务服务
    /// </summary>
    public class JobService : IJobService
    {
        IJobBusiness jobBusiness = null;

        public JobService(IJobBusiness jobBusiness)
        {
            this.jobBusiness = jobBusiness;
        }

        #region 保存工作任务

        /// <summary>
        /// 保存工作任务
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result<JobDto> SaveJob(SaveJobCmdDto saveInfo)
        {
            return jobBusiness.SaveJob(saveInfo);
        }

        #endregion

        #region 获取工作任务

        /// <summary>
        /// 获取工作任务
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public JobDto GetJob(JobFilterDto filter)
        {
            return jobBusiness.GetJob(filter);
        }

        #endregion

        #region 获取工作任务列表

        /// <summary>
        /// 获取工作任务列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<JobDto> GetJobList(JobFilterDto filter)
        {
            return jobBusiness.GetJobList(filter);
        }

        #endregion

        #region 获取工作任务分页

        /// <summary>
        /// 获取工作任务分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<JobDto> GetJobPaging(JobFilterDto filter)
        {
            return jobBusiness.GetJobPaging(filter);
        }

        #endregion

        #region 删除工作任务

        /// <summary>
        /// 删除工作任务
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJob(DeleteJobCmdDto deleteInfo)
        {
            return jobBusiness.DeleteJob(deleteInfo);
        }

        #endregion

        #region 修改工作任务运行状态

        /// <summary>
        /// 修改工作任务运行状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        public Result ModifyJobRunState(ModifyJobRunStateCmdDto stateInfo)
        {
            return jobBusiness.ModifyJobRunState(stateInfo);
        }

        #endregion
    }
}
