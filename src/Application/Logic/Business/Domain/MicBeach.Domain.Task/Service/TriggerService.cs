using MicBeach.Domain.Task.Model;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Util.Paging;
using MicBeach.Util;
using MicBeach.Util.CustomerException;
using MicBeach.Util.ExpressionUtil;

namespace MicBeach.Domain.Task.Service
{
    /// <summary>
    /// 执行计划服务
    /// </summary>
    public static class TriggerService
    {
        static ITriggerRepository triggerRepository = ContainerManager.Resolve<ITriggerRepository>();

        #region 保存执行计划

        /// <summary>
        /// 保存任务执行计划
        /// </summary>
        /// <param name="trigger">执行计划信息</param>
        /// <returns>执行结果</returns>
        public static void SaveTrigger(Trigger trigger)
        {
            if (trigger == null)
            {
                return;
            }
            if (trigger.Id.IsNullOrEmpty())
            {
                AddTrigger(trigger);
            }
            else
            {
                UpdateTrigger(trigger);
            }
        }

        /// <summary>
        /// 添加任务执行计划
        /// </summary>
        /// <param name="trigger">执行计划</param>
        /// <returns>执行结果</returns>
        static void AddTrigger(Trigger trigger)
        {
            trigger.Save();
        }

        /// <summary>
        /// 更新任务执行计划
        /// </summary>
        /// <param name="trigger">执行计划</param>
        /// <returns>执行结果</returns>
        static void UpdateTrigger(Trigger trigger)
        {
            var nowTrigger = triggerRepository.Get(QueryFactory.Create<TriggerQuery>(c => c.Id == trigger.Id));
            if (nowTrigger == null)
            {
                throw new AppException("请指定正确的要操作的信息");
            }
            IEnumerable<string> excludePropertys= ExpressionHelper.GetExpressionPropertyNames<Trigger>(t=>t.StartTime,t=>t.EndTime,t=>t.PrevFireTime,t=>t.NextFireTime);
            nowTrigger.ModifyFromOtherTrigger(trigger, excludePropertys);
            nowTrigger.Save();
        }

        #endregion

        #region 删除执行计划

        /// <summary>
        /// 删除执行计划
        /// </summary>
        /// <param name="triggers">执行计划信息</param>
        public static void DeleteTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            triggerRepository.Remove(triggers.ToArray());
        }

        /// <summary>
        /// 删除执行计划
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        public static void DeleteTrigger(IEnumerable<string> triggerIds)
        {
            if (triggerIds.IsNullOrEmpty())
            {
                return;
            }
            DeleteTrigger(triggerIds.Select(c => Trigger.CreateTrigger(c)));
        }

        /// <summary>
        /// 修改执行计划状态
        /// </summary>
        /// <param name="triggers">计划信息</param>
        public static void ModifyTriggerState(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            var triggerIds = triggers.Select(c => c.Id).Distinct();
            var nowTriggers = triggerRepository.GetList(QueryFactory.Create<TriggerQuery>(c => triggerIds.Contains(c.Id)));
            if (nowTriggers.IsNullOrEmpty())
            {
                return;
            }
            foreach (var trigger in nowTriggers)
            {
                var newTrigger = triggers.FirstOrDefault(c => c.Id == trigger.Id);
                if (newTrigger == null)
                {
                    continue;
                }
                trigger.State = newTrigger.State;
                trigger.Save();
            }
        }

        #endregion

        #region 获取任务执行计划

        /// <summary>
        /// 获取任务执行计划
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>执行计划对象</returns>
        public static Trigger GetTrigger(IQuery query)
        {
            return triggerRepository.Get(query);
        }

        /// <summary>
        /// 根据编号获取任务执行计划
        /// </summary>
        /// <param name="id">计划编号</param>
        /// <returns>任务执行计划</returns>
        public static Trigger GetTrigger(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return null;
            }
            IQuery query = QueryFactory.Create<TriggerQuery>(c => c.Id == id);
            return triggerRepository.Get(query);
        }

        #endregion

        #region 获取任务执行计划列表

        /// <summary>
        /// 获取任务执行计划列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>执行计划列表</returns>
        public static List<Trigger> GetTriggerList(IQuery query)
        {
            var triggerList = triggerRepository.GetList(query);
            LoadOtherObjectData(triggerList, query);
            return triggerList;
        }

        /// <summary>
        /// 根据计划编号获取计划列表
        /// </summary>
        /// <param name="ids">执行计划编号</param>
        /// <returns>执行计划列表</returns>
        public static List<Trigger> GetTriggerList(IEnumerable<string> ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return new List<Trigger>(0);
            }
            IQuery query = QueryFactory.Create<TriggerQuery>(c => ids.Contains(c.Id));
            return GetTriggerList(query);
        }

        #endregion

        #region 获取任务执行计划分页

        /// <summary>
        /// 获取任务执行计划分页
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns>执行计划分页</returns>
        public static IPaging<Trigger> GetTriggerPaging(IQuery query)
        {
            var triggerPaging = triggerRepository.GetPaging(query);
            var triggerList = LoadOtherObjectData(triggerPaging, query);
            return new Paging<Trigger>(triggerPaging.Page, triggerPaging.PageSize, triggerPaging.TotalCount, triggerList);
        }

        #endregion

        #region 加载其它对象数据

        /// <summary>
        /// 记载其它对象数据
        /// </summary>
        /// <param name="triggers">执行计划数据</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        static List<Trigger> LoadOtherObjectData(IEnumerable<Trigger> triggers, IQuery query)
        {
            if (triggers.IsNullOrEmpty() || query == null)
            {
                return triggers.ToList();
            }

            #region 工作任务

            List<Job> jobList = null;
            if (query.AllowLoad<Trigger>(t => t.Job))
            {
                var jobIds = triggers.Select(c => c.Job?.Id).Distinct().ToList();
                jobList = JobService.GetJobList(jobIds);
            }

            #endregion

            #region 附加条件

            List<TriggerCondition> conditionList = null;
            if (query.AllowLoad<Trigger>(c => c.Condition))
            {
                var triggerIds = triggers.Select(c => c.Id).Distinct().ToList();
                conditionList = TriggerConditionService.GetTriggerConditionList(triggers);
            }

            #endregion

            foreach (var trigger in triggers)
            {
                if (!jobList.IsNullOrEmpty())
                {
                    trigger.SetJob(jobList.FirstOrDefault(c => c.Id == trigger.Job?.Id), true);
                }
                if (!conditionList.IsNullOrEmpty())
                {
                    trigger.SetCondition(conditionList.FirstOrDefault(c => c.TriggerId == trigger.Id), true);
                }
            }
            return triggers.ToList();
        }

        #endregion
    }
}
