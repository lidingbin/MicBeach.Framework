using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Response;

namespace MicBeach.BusinessInterface.Task
{
    /// <summary>
    /// 任务执行计划业务接口
    /// </summary>
    public interface ITriggerBusiness
    {
        #region 保存任务执行计划

        /// <summary>
        /// 保存任务执行计划
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        Result<TriggerDto> SaveTrigger(SaveTriggerCmdDto saveInfo);

        #endregion

        #region 获取任务执行计划

        /// <summary>
        /// 获取任务执行计划
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        TriggerDto GetTrigger(TriggerFilterDto filter);

        #endregion

        #region 获取任务执行计划列表

        /// <summary>
        /// 获取任务执行计划列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        List<TriggerDto> GetTriggerList(TriggerFilterDto filter);

        #endregion

        #region 获取任务执行计划分页

        /// <summary>
        /// 获取任务执行计划分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IPaging<TriggerDto> GetTriggerPaging(TriggerFilterDto filter);

        #endregion

        #region 删除任务执行计划

        /// <summary>
        /// 删除任务执行计划
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        Result DeleteTrigger(DeleteTriggerCmdDto deleteInfo);

        #endregion

        #region 修改执行计划状态

        /// <summary>
        /// 修改执行计划状态
        /// </summary>
        /// <param name="stateInfo">状态信息</param>
        /// <returns></returns>
        Result ModifyTriggerState(ModifyTriggerStateCmdDto stateInfo);

        #endregion

    }
}
