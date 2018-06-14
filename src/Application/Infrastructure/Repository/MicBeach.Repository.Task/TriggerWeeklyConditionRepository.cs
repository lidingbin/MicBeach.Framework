using MicBeach.Develop.CQuery;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using MicBeach.Entity.Task;
using MicBeach.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Domain.Task.Repository;
using MicBeach.Query.Task;
using MicBeach.Develop.Command;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 星期计划存储
    /// </summary>
    public class TriggerWeeklyConditionRepository : DefaultRepository<TriggerWeeklyCondition, TriggerWeeklyConditionEntity, ITriggerWeeklyConditionDataAccess>, ITriggerWeeklyConditionRepository
    {
        #region 重载功能

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="objDatas">要保存的数据</param>
        protected override void ExecuteSave(params TriggerWeeklyCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerWeeklyConditionEntity> weekEntityList = new List<TriggerWeeklyConditionEntity>();
            foreach (var condition in objDatas)
            {
                if (condition == null || condition.Days.IsNullOrEmpty())
                {
                    continue;
                }
                weekEntityList.AddRange(condition.Days.Select(c =>
                {
                    var entity = c.MapTo<TriggerWeeklyConditionEntity>();
                    entity.TriggerId = condition.TriggerId;
                    return entity;
                }).ToList());
            }
            //移除当前的条件
            List<string> triggerIds = objDatas.Select(c => c.TriggerId).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerWeeklyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            Remove(removeQuery);
            //添加新的条件
            Add(weekEntityList.Distinct(new EntityCompare<TriggerWeeklyConditionEntity>()).ToArray());
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="objDatas">要移除的数据</param>
        protected override void ExecuteRemove(params TriggerWeeklyCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerWeeklyConditionEntity> weekEntityList = new List<TriggerWeeklyConditionEntity>();
            foreach (var obj in objDatas)
            {
                if (obj == null || obj.Days.IsNullOrEmpty())
                {
                    continue;
                }
                weekEntityList.AddRange(obj.Days.Select(c => 
                {
                    var entity=c.MapTo<TriggerWeeklyConditionEntity>();
                    entity.TriggerId = obj.TriggerId;
                    return entity;
                }));
            }
            Remove(weekEntityList);
        }

        /// <summary>
        /// 移除计划附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        public void RemoveTriggerConditionFromTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            List<string> triggerIds = triggers.Select(c => c.Id).Distinct().ToList();
            Remove(QueryFactory.Create<TriggerWeeklyConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override TriggerWeeklyCondition GetData(IQuery query)
        {
            List<TriggerWeeklyConditionEntity> weekEntityList = dataAccess.GetList(query);
            if (weekEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = weekEntityList.First().TriggerId;
            TriggerWeeklyCondition weekCondtion = new TriggerWeeklyCondition(triggerId)
            {
                Days = weekEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<WeeklyConditionDay>()).ToList()
            };
            weekCondtion.MarkStored();
            return weekCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override List<TriggerWeeklyCondition> GetDataList(IQuery query)
        {
            List<TriggerWeeklyConditionEntity> weekEntityList = dataAccess.GetList(query);
            if (weekEntityList.IsNullOrEmpty())
            {
                return new List<TriggerWeeklyCondition>(0);
            }
            IEnumerable<string> triggerIds = weekEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerWeeklyCondition> weekConditions = new List<TriggerWeeklyCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerWeeklyCondition weekCondtion = new TriggerWeeklyCondition(triggerId)
                {
                    Days = weekEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<WeeklyConditionDay>()).ToList()
                };
                weekCondtion.MarkStored();
                weekConditions.Add(weekCondtion);
            }
            return weekConditions;
        }

        #endregion

        #endregion
    }
}
