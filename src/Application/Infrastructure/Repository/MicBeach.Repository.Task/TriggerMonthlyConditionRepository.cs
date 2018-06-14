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
    /// 每月日期计划存储
    /// </summary>
    public class TriggerMonthlyConditionRepository : DefaultRepository<TriggerMonthlyCondition, TriggerMonthlyConditionEntity, ITriggerMonthlyConditionDataAccess>, ITriggerMonthlyConditionRepository
    {
        #region 重载功能

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="objDatas">要保存的数据</param>
        protected override void ExecuteSave(params TriggerMonthlyCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerMonthlyConditionEntity> monthlyEntityList = new List<TriggerMonthlyConditionEntity>();
            foreach (var condition in objDatas)
            {
                if (condition == null || condition.Days.IsNullOrEmpty())
                {
                    continue;
                }
                monthlyEntityList.AddRange(condition.Days.Select(c =>
                {
                    var entity = c.MapTo<TriggerMonthlyConditionEntity>();
                    entity.TriggerId = condition.TriggerId;
                    return entity;
                }).ToList());
            }
            //移除当前的条件
            List<string> triggerIds = objDatas.Select(c => c.TriggerId).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerMonthlyConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            Remove(removeQuery);
            //添加新的条件
            Add(monthlyEntityList.Distinct(new EntityCompare<TriggerMonthlyConditionEntity>()).ToArray());
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="objDatas">要移除的数据</param>
        protected override void ExecuteRemove(params TriggerMonthlyCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerMonthlyConditionEntity> monthlyEntityList = new List<TriggerMonthlyConditionEntity>();
            foreach (var obj in objDatas)
            {
                if (obj == null || obj.Days.IsNullOrEmpty())
                {
                    continue;
                }
                monthlyEntityList.AddRange(obj.Days.Select(c => 
                {
                    var entity=c.MapTo<TriggerMonthlyConditionEntity>();
                    entity.TriggerId = obj.TriggerId;
                    return entity;
                }));
            }
            Remove(monthlyEntityList);
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
            Remove(QueryFactory.Create<TriggerMonthlyConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override TriggerMonthlyCondition GetData(IQuery query)
        {
            List<TriggerMonthlyConditionEntity> monthlyEntityList = dataAccess.GetList(query);
            if (monthlyEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = monthlyEntityList.First().TriggerId;
            TriggerMonthlyCondition monthlyCondtion = new TriggerMonthlyCondition(triggerId)
            {
                Days = monthlyEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<MonthConditionDay>()).ToList()
            };
            monthlyCondtion.MarkStored();
            return monthlyCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override List<TriggerMonthlyCondition> GetDataList(IQuery query)
        {
            List<TriggerMonthlyConditionEntity> monthlyEntityList = dataAccess.GetList(query);
            if (monthlyEntityList.IsNullOrEmpty())
            {
                return new List<TriggerMonthlyCondition>(0);
            }
            IEnumerable<string> triggerIds = monthlyEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerMonthlyCondition> monthlyConditions = new List<TriggerMonthlyCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerMonthlyCondition monthlyCondtion = new TriggerMonthlyCondition(triggerId)
                {
                    Days = monthlyEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<MonthConditionDay>()).ToList()
                };
                monthlyCondtion.MarkStored();
                monthlyConditions.Add(monthlyCondtion);
            }
            return monthlyConditions;
        }

        #endregion

        #endregion
    }
}
