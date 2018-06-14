using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.DTO.Task.Cmd;
using MicBeach.DTO.Task.Query;
using MicBeach.DTO.Task.Query.Filter;
using MicBeach.Query.Task;
using MicBeach.Domain.Task.Repository;
using MicBeach.BusinessInterface.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Util;
using MicBeach.Domain.Task.Model;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Domain.Task.Service;
using MicBeach.Util.Response;

namespace MicBeach.Business.Task
{
    /// <summary>
    /// 任务执行日志业务
    /// </summary>
    public class ExecuteLogBusiness : IExecuteLogBusiness
    {
        public ExecuteLogBusiness()
        {
        }

        #region 保存任务执行日志

        /// <summary>
        /// 保存任务执行日志
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result SaveExecuteLog(SaveExecuteLogCmdDto saveInfo)
        {
            if (saveInfo == null || saveInfo.ExecuteLogs.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定要保存的信息");
            }
            List<ExecuteLog> logs = saveInfo.ExecuteLogs.Select(c =>c.MapTo<ExecuteLog>()).ToList();
            using (var businessWork = UnitOfWork.Create())
            {
                ExecuteLogService.SaveExecuteLog(logs);
                var commitResult = businessWork.Commit();
                if (commitResult.ExecutedSuccess)
                {
                    return Result.SuccessResult("保存成功");
                }
                return Result.FailedResult("保存失败");
            }
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
            var executeLog = ExecuteLogService.GetExecuteLog(CreateQueryObject(filter));
            return executeLog.MapTo<ExecuteLogDto>();
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
            var executeLogList = ExecuteLogService.GetExecuteLogList(CreateQueryObject(filter));
            return executeLogList.Select(c => c.MapTo<ExecuteLogDto>()).ToList();
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
            var executeLogPaging = ExecuteLogService.GetExecuteLogPaging(CreateQueryObject(filter));
            return executeLogPaging.ConvertTo<ExecuteLogDto>();
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
            #region 参数判断

            //if (deleteInfo == null || deleteInfo.ExecuteLogIds.IsNullOrEmpty())
            //{
            //    return Result.FailedResult("没有指定要删除的任务执行日志");
            //}

            #endregion

            //删除逻辑
            //var commitResult = UnitOfWork.Commit();
            //return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            return null;
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(ExecuteLogFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = QueryFactory.Create(filter);

            #region 筛选条件

            if (filter.Id.HasValue)
            {
                query.Equal<ExecuteLogQuery>(c => c.Id, filter.Id.Value);
            }
            if (!filter.Job.IsNullOrEmpty())
            {
                query.Equal<ExecuteLogQuery>(c => c.Job, filter.Job);
            }
            if (!filter.Trigger.IsNullOrEmpty())
            {
                query.Equal<ExecuteLogQuery>(c => c.Trigger, filter.Trigger);
            }
            if (!filter.Server.IsNullOrEmpty())
            {
                query.Equal<ExecuteLogQuery>(c => c.Server, filter.Server);
            }
            if (filter.BeginTime.HasValue)
            {
                query.GreaterThanOrEqual<ExecuteLogQuery>(c => c.BeginTime, filter.BeginTime.Value);
            }
            if (filter.EndTime.HasValue)
            {
                query.LessThanOrEqual<ExecuteLogQuery>(c => c.EndTime, filter.EndTime.Value);
            }
            if (filter.RecordTime.HasValue)
            {
                query.Equal<ExecuteLogQuery>(c => c.RecordTime, filter.RecordTime.Value);
            }
            if (filter.State.HasValue)
            {
                query.Equal<ExecuteLogQuery>(c => c.State, filter.State.Value);
            }
            if (!filter.Message.IsNullOrEmpty())
            {
                query.Equal<ExecuteLogQuery>(c => c.Message, filter.Message);
            }

            #endregion

            #region 数据加载

            if (filter.LoadServer)
            {
                query.SetLoadPropertys<ExecuteLog>(true, c => c.Server);
            }
            if (filter.LoadJob)
            {
                query.SetLoadPropertys<ExecuteLog>(true, c => c.Job);
            }
            if (filter.LoadTrigger)
            {
                query.SetLoadPropertys<ExecuteLog>(true, c => c.Trigger);
            }

            #endregion

            #region 排序

            query.Desc<ExecuteLogQuery>(c => c.BeginTime);

            #endregion

            return query;
        }

        #endregion
    }
}
