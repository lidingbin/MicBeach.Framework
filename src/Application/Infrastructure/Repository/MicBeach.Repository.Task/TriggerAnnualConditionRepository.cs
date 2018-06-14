using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.CQuery;
using MicBeach.Domain.Task.Repository;
using MicBeach.Query.Task;
using MicBeach.Develop.Command;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 年度计划存储
    /// </summary>
    public class TriggerAnnualConditionRepository : DefaultRepository<TriggerAnnualCondition, TriggerAnnualConditionEntity, ITriggerAnnualConditionDataAccess>, ITriggerAnnualConditionRepository
    {
        #region 重载功能

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="objDatas">要保存的数据</param>
        protected override void ExecuteSave(params TriggerAnnualCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerAnnualConditionEntity> annualDaysEntityList = new List<TriggerAnnualConditionEntity>();
            foreach (var condition in objDatas)
            {
                if (condition == null || condition.Days.IsNullOrEmpty())
                {
                    continue;
                }
                annualDaysEntityList.AddRange(condition.Days.Select(c =>
                {
                    var entity = c.MapTo<TriggerAnnualConditionEntity>();
                    entity.TriggerId = condition.TriggerId;
                    return entity;
                }).ToList());
            }
            //移除当前的条件
            List<string> triggerIds = objDatas.Select(c => c.TriggerId).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerAnnualConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            Remove(removeQuery);
            //添加信息的计划
            Add(annualDaysEntityList.Distinct(new EntityCompare<TriggerAnnualConditionEntity>()).ToArray());
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="objDatas">要移除的数据</param>
        protected override void ExecuteRemove(params TriggerAnnualCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerAnnualConditionEntity> annualDaysEntityList = new List<TriggerAnnualConditionEntity>();
            foreach (var obj in objDatas)
            {
                if (obj == null || obj.Days.IsNullOrEmpty())
                {
                    continue;
                }
                annualDaysEntityList.AddRange(obj.Days.Select(c =>
                {
                    var entity = c.MapTo<TriggerAnnualConditionEntity>();
                    entity.TriggerId = obj.TriggerId;
                    return entity;
                }));
            }
            Remove(annualDaysEntityList);
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
            Remove(QueryFactory.Create<TriggerAnnualConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override TriggerAnnualCondition GetData(IQuery query)
        {
            List<TriggerAnnualConditionEntity> annualDaysEntityList = dataAccess.GetList(query);
            if (annualDaysEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = annualDaysEntityList.First().TriggerId;
            TriggerAnnualCondition annualCondtion = new TriggerAnnualCondition(triggerId)
            {
                Days = annualDaysEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<AnnualConditionDay>()).ToList()
            };
            annualCondtion.MarkStored();
            return annualCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override List<TriggerAnnualCondition> GetDataList(IQuery query)
        {
            List<TriggerAnnualConditionEntity> annualDaysEntityList = dataAccess.GetList(query);
            if (annualDaysEntityList.IsNullOrEmpty())
            {
                return new List<TriggerAnnualCondition>(0);
            }
            IEnumerable<string> triggerIds = annualDaysEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerAnnualCondition> annualConditions = new List<TriggerAnnualCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerAnnualCondition annualCondtion = new TriggerAnnualCondition(triggerId)
                {
                    Days = annualDaysEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<AnnualConditionDay>()).ToList()
                };
                annualCondtion.MarkStored();
                annualConditions.Add(annualCondtion);
            }
            return annualConditions;
        }

        #endregion

        #endregion
    }
}
