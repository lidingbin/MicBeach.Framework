using MicBeach.ServiceInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util;
using MicBeach.BusinessInterface.Task;
using MicBeach.Util.Response;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util.Paging;

namespace MicBeach.Service.Task
{
    /// <summary>
    /// 任务执行计划服务
    /// </summary>
    public class TriggerService : ITriggerService
    {
        ITriggerBusiness triggerBusiness = null;

        public TriggerService(ITriggerBusiness triggerBusiness)
        {
            this.triggerBusiness = triggerBusiness;
        }

        #region 保存任务执行计划

        /// <summary>
        /// 保存任务执行计划
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result<TriggerDto> SaveTrigger(SaveTriggerCmdDto saveInfo)
        {
            return triggerBusiness.SaveTrigger(saveInfo);
        }

        #endregion

        #region 获取任务执行计划

        /// <summary>
        /// 获取任务执行计划
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public TriggerDto GetTrigger(TriggerFilterDto filter)
        {
            return triggerBusiness.GetTrigger(filter);
        }

        #endregion

        #region 获取任务执行计划列表

        /// <summary>
        /// 获取任务执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<TriggerDto> GetTriggerList(TriggerFilterDto filter)
        {
            return triggerBusiness.GetTriggerList(filter);
        }

        #endregion

        #region 获取任务执行计划分页

        /// <summary>
        /// 获取任务执行计划分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<TriggerDto> GetTriggerPaging(TriggerFilterDto filter)
        {
            return triggerBusiness.GetTriggerPaging(filter);
        }

        #endregion

        #region 删除任务执行计划

        /// <summary>
        /// 删除任务执行计划
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteTrigger(DeleteTriggerCmdDto deleteInfo)
        {
            return triggerBusiness.DeleteTrigger(deleteInfo);
        }

        #endregion

        #region 修改执行计划状态

        /// <summary>
        /// 修改执行计划状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        public Result ModifyTriggerState(ModifyTriggerStateCmdDto stateInfo)
        {
            return triggerBusiness.ModifyTriggerState(stateInfo);
        }

        #endregion
    }
}
