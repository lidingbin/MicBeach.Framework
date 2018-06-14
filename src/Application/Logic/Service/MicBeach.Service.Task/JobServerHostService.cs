using MicBeach.ServiceInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util;
using MicBeach.BusinessInterface.Task;
using MicBeach.Util.Response;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util.Paging;

namespace MicBeach.Service.Task
{
    /// <summary>
    /// 工作承载节点服务
    /// </summary>
    public class JobServerHostService : IJobServerHostService
    {
        IJobServerHostBusiness jobServerHostBusiness = null;

        public JobServerHostService(IJobServerHostBusiness jobServerHostBusiness)
        {
            this.jobServerHostBusiness = jobServerHostBusiness;
        }

        #region 保存工作承载节点

        /// <summary>
        /// 保存工作承载节点
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result SaveJobServerHost(SaveJobServerHostCmdDto saveInfo)
        {
            return jobServerHostBusiness.SaveJobServerHost(saveInfo);
        }

        #endregion

        #region 获取工作承载节点

        /// <summary>
        /// 获取工作承载节点
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public JobServerHostDto GetJobServerHost(JobServerHostFilterDto filter)
        {
            return jobServerHostBusiness.GetJobServerHost(filter);
        }

        #endregion

        #region 获取工作承载节点列表

        /// <summary>
        /// 获取工作承载节点列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<JobServerHostDto> GetJobServerHostList(JobServerHostFilterDto filter)
        {
            return jobServerHostBusiness.GetJobServerHostList(filter);
        }

        #endregion

        #region 获取工作承载节点分页

        /// <summary>
        /// 获取工作承载节点分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<JobServerHostDto> GetJobServerHostPaging(JobServerHostFilterDto filter)
        {
            return jobServerHostBusiness.GetJobServerHostPaging(filter);
        }

        #endregion

        #region 修改承载服务运行状态

        /// <summary>
        /// 修改承载服务运行状态
        /// </summary>
        /// <param name="modifyInfo">修改信息</param>
        /// <returns></returns>
        public Result ModifyRunState(ModifyJobServerHostRunStateCmdDto modifyInfo)
        {
            return jobServerHostBusiness.ModifyRunState(modifyInfo);
        }

        #endregion

        #region 删除工作承载节点

        /// <summary>
        /// 删除工作承载节点
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteJobServerHost(DeleteJobServerHostCmdDto deleteInfo)
        {
            return jobServerHostBusiness.DeleteJobServerHost(deleteInfo);
        }

        #endregion

    }
}
