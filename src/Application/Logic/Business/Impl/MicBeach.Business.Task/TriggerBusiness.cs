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
using MicBeach.CTask;
using MicBeach.Domain.Task.Service;
using MicBeach.Util.Response;

namespace MicBeach.Business.Task
{
    /// <summary>
    /// 任务执行计划业务
    /// </summary>
    public class TriggerBusiness : ITriggerBusiness
    {
        public TriggerBusiness()
        {
        }

        #region 保存任务执行计划

        /// <summary>
        /// 保存任务执行计划
        /// </summary>
        /// <param name="saveInfo">保存信息</param>
        /// <returns>执行结果</returns>
        public Result<TriggerDto> SaveTrigger(SaveTriggerCmdDto saveInfo)
        {
            using (var businessWork = UnitOfWork.Create())
            {
                if (saveInfo == null || saveInfo.Trigger == null)
                {
                    return Result<TriggerDto>.FailedResult("保存信息为空");
                }
                Result<TriggerDto> result = null;
                var trigger = saveInfo.Trigger.MapTo<Trigger>();
                TriggerService.SaveTrigger(trigger);//保存执行计划
                var commitResult = businessWork.Commit();
                if (commitResult.ExecutedSuccess)
                {
                    result = Result<TriggerDto>.SuccessResult("保存成功");
                    result.Data = trigger.MapTo<TriggerDto>();
                }
                else
                {
                    result = Result<TriggerDto>.FailedResult("保存失败");
                }

                return result;
            }
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
            var trigger = TriggerService.GetTrigger(CreateQueryObject(filter));
            return trigger.MapTo<TriggerDto>();
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
            var triggerList = TriggerService.GetTriggerList(CreateQueryObject(filter));
            return triggerList.Select(c => c.MapTo<TriggerDto>()).ToList();
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
            var triggerPaging = TriggerService.GetTriggerPaging(CreateQueryObject(filter));
            return triggerPaging.ConvertTo<TriggerDto>();
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
            using (var businessWork = UnitOfWork.Create())
            {
                #region 参数判断

                if (deleteInfo == null || deleteInfo.TriggerIds.IsNullOrEmpty())
                {
                    return Result.FailedResult("没有指定要删除的任务执行计划");
                }

                #endregion

                //删除逻辑
                TriggerService.DeleteTrigger(deleteInfo.TriggerIds);
                var commitResult = businessWork.Commit();
                return commitResult.ExecutedSuccess ? Result.SuccessResult("删除成功") : Result.FailedResult("删除失败");
            }
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
            if (stateInfo == null || stateInfo.Triggers.IsNullOrEmpty())
            {
                return Result.FailedResult("没有指定任何要修改的信息");
            }
            using (var businessWork = UnitOfWork.Create())
            {
                TriggerService.ModifyTriggerState(stateInfo.Triggers.Select(c => c.MapTo<Trigger>()));
                var commitResult = businessWork.Commit();
                var result = commitResult.ExecutedSuccess ? Result.SuccessResult("修改成功") : Result.FailedResult("修改失败");

                return result;
            }
        }

        #endregion

        #region 根据查询条件生成查询对象

        /// <summary>
        /// 根据查询条件生成查询对象
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        IQuery CreateQueryObject(TriggerFilterDto filter, bool useBaseFilter = false)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery query = null;
            if (useBaseFilter)
            {
                query = QueryFactory.Create(filter);

                #region 筛选条件

                if (!filter.Ids.IsNullOrEmpty())
                {
                    query.In<TriggerQuery>(c => c.Id, filter.Ids);
                }
                if (!filter.Name.IsNullOrEmpty())
                {
                    query.Like<TriggerQuery>(c => c.Name, filter.Name);
                }
                if (!filter.Description.IsNullOrEmpty())
                {
                    query.Equal<TriggerQuery>(c => c.Description, filter.Description);
                }
                if (!filter.Job.IsNullOrEmpty())
                {
                    query.Equal<TriggerQuery>(c => c.Job, filter.Job);
                }
                if (filter.ApplyTo.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.ApplyTo, filter.ApplyTo.Value);
                }
                if (filter.PrevFireTime.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.PrevFireTime, filter.PrevFireTime.Value);
                }
                if (filter.NextFirTime.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.NextFirTime, filter.NextFirTime.Value);
                }
                if (filter.Priority.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.Priority, filter.Priority.Value);
                }
                if (filter.State.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.State, filter.State.Value);
                }
                if (filter.Type.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.Type, filter.Type.Value);
                }
                if (filter.StartTime.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.StartTime, filter.StartTime.Value);
                }
                if (filter.EndTime.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.EndTime, filter.EndTime.Value);
                }
                if (filter.MisFireType.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.MisFireType, filter.MisFireType.Value);
                }
                if (filter.FireTotalCount.HasValue)
                {
                    query.Equal<TriggerQuery>(c => c.FireTotalCount, filter.FireTotalCount.Value);
                }

                #endregion

                #region 数据加载

                if (filter.LoadJob)
                {
                    query.SetLoadPropertys<Trigger>(true, c => c.Job);
                }
                if (filter.LoadCondition)
                {
                    query.SetLoadPropertys<Trigger>(true, c => c.Condition);
                }

                #endregion
            }
            else
            {
                if (filter is ServerScheduleTriggerFilterDto)
                {
                    query = CreateQueryObject(filter as ServerScheduleTriggerFilterDto);
                }
                else
                {
                    query = CreateQueryObject(filter, true);
                }
            }
            return query;
        }

        /// <summary>
        /// 生成服务调度计划查询对象
        /// </summary>
        /// <param name="filter">查询信息</param>
        /// <returns></returns>
        IQuery CreateQueryObject(ServerScheduleTriggerFilterDto filter)
        {
            if (filter == null)
            {
                return null;
            }
            IQuery triggerQuery = CreateQueryObject(filter, true) ?? QueryFactory.Create<TriggerQuery>();

            #region 任务筛选

            IQuery jobQuery = this.Instance<IJobBusiness>().CreateQueryObject(filter.JobFilter);
            if (jobQuery != null && jobQuery.Criterias.Count > 0)
            {
                jobQuery.AddQueryFields<JobQuery>(c => c.Id);
                triggerQuery.And<TriggerQuery>(c => c.Job, CriteriaOperator.In, jobQuery);
            }

            #endregion

            #region 服务筛选

            IQuery serverQuery = this.Instance<IServerNodeBusiness>().CreateQueryObject(filter.ServerFilter);
            if (serverQuery != null && serverQuery.Criterias.Count > 0)
            {
                IQuery triggerServerFilterQuery = QueryFactory.Create();

                serverQuery.AddQueryFields<ServerNodeQuery>(c => c.Id);
                IQuery triggerServerQuery = QueryFactory.Create<TriggerServerQuery>();
                triggerServerQuery.And<TriggerServerQuery>(c => c.Server, CriteriaOperator.In, serverQuery);
                triggerServerQuery.AddQueryFields<TriggerServerQuery>(c => c.Trigger);
                triggerServerFilterQuery.And<TriggerQuery>(c => c.Id, CriteriaOperator.In, triggerServerQuery);

                //服务承载任务针对所有的执行计划
                IQuery serverJobHostQuery = QueryFactory.Create<JobServerHostQuery>();
                serverJobHostQuery.And<JobServerHostQuery>(c => c.Server, CriteriaOperator.In, serverQuery);
                serverJobHostQuery.AddQueryFields<JobServerHostQuery>(c => c.Job);
                IQuery applyToAllServerQuery = QueryFactory.Create();
                applyToAllServerQuery.And<TriggerQuery>(c => c.Job, CriteriaOperator.In, serverJobHostQuery);
                applyToAllServerQuery.And<TriggerQuery>(c => c.ApplyTo == TaskTriggerApplyTo.所有);

                triggerServerFilterQuery.Or(applyToAllServerQuery);
                triggerQuery.And(triggerServerFilterQuery);
            }

            #endregion

            return triggerQuery;
        }

        #endregion
    }
}
