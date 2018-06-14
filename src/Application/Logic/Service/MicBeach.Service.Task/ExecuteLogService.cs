using MicBeach.ServiceInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util;
using MicBeach.BusinessInterface.Task;
using MicBeach.DTO.Task.Cmd;
using MicBeach.Util.Response;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.DTO.Task.Query;
using MicBeach.Util.Paging;

namespace MicBeach.Service.Task
{
    /// <summary>
    /// 任务执行日志服务
    /// </summary>
    public class ExecuteLogService : IExecuteLogService
    {
        IExecuteLogBusiness executeLogBusiness = null;

        public ExecuteLogService(IExecuteLogBusiness executeLogBusiness)
        {
            this.executeLogBusiness = executeLogBusiness;
        }

        #region 保存任务执行日志

        /// <summary>
        /// 保存任务执行日志
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result SaveExecuteLog(SaveExecuteLogCmdDto saveInfo)
        {
            return executeLogBusiness.SaveExecuteLog(saveInfo);
        }

        #endregion

        #region 获取任务执行日志

        /// <summary>
        /// 获取任务执行日志
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public ExecuteLogDto GetExecuteLog(ExecuteLogFilterDto filter)
        {
            return executeLogBusiness.GetExecuteLog(filter);
        }

        #endregion

        #region 获取任务执行日志列表

        /// <summary>
        /// 获取任务执行日志列表
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public List<ExecuteLogDto> GetExecuteLogList(ExecuteLogFilterDto filter)
        {
            return executeLogBusiness.GetExecuteLogList(filter);
        }

        #endregion

        #region 获取任务执行日志分页

        /// <summary>
        /// 获取任务执行日志分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        public IPaging<ExecuteLogDto> GetExecuteLogPaging(ExecuteLogFilterDto filter)
        {
            return executeLogBusiness.GetExecuteLogPaging(filter);
        }

        #endregion

        #region 删除任务执行日志

        /// <summary>
        /// 删除任务执行日志
        /// </summary>
        /// <param name="deleteInfo">删除信息</param>
        /// <returns>执行结果</returns>
        public Result DeleteExecuteLog(DeleteExecuteLogCmdDto deleteInfo)
        {
            return executeLogBusiness.DeleteExecuteLog(deleteInfo);
        }

        #endregion

    }
}
