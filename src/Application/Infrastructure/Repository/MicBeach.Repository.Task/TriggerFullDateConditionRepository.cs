using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Develop.CQuery;
using MicBeach.Domain.Task.Repository;
using MicBeach.Query.Task;
using MicBeach.Develop.Command;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 固定日期存储
    /// </summary>
    public class TriggerFullDateConditionRepository : DefaultRepository<TriggerFullDateCondition, TriggerFullDateConditionEntity, ITriggerFullDateConditionDataAccess>, ITriggerFullDateConditionRepository
    {
        #region 重载功能

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="objDatas">要保存的数据</param>
        protected override void ExecuteSave(params TriggerFullDateCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerFullDateConditionEntity> fullDateEntityList = new List<TriggerFullDateConditionEntity>();
            foreach (var condition in objDatas)
            {
                if (condition == null || condition.Dates.IsNullOrEmpty())
                {
                    continue;
                }
                fullDateEntityList.AddRange(condition.Dates.Select(c =>
                {
                    var entity = c.MapTo<TriggerFullDateConditionEntity>();
                    entity.TriggerId = condition.TriggerId;
                    return entity;
                }).ToList());
            }
            //移除当前的条件
            List<string> triggerIds = objDatas.Select(c => c.TriggerId).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerFullDateConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            Remove(removeQuery);
            //添加新的条件
            Add(fullDateEntityList.Distinct(new EntityCompare<TriggerFullDateConditionEntity>()).ToArray());
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="objDatas">要移除的数据</param>
        protected override void ExecuteRemove(params TriggerFullDateCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerFullDateConditionEntity> fullDateConditionList = new List<TriggerFullDateConditionEntity>();
            foreach (var obj in objDatas)
            {
                if (obj == null || obj.Dates.IsNullOrEmpty())
                {
                    continue;
                }
                fullDateConditionList.AddRange(obj.Dates.Select(c =>
                {
                    var entity = c.MapTo<TriggerFullDateConditionEntity>();
                    entity.TriggerId = obj.TriggerId;
                    return entity;
                }));
            }
            Remove(fullDateConditionList);
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
            Remove(QueryFactory.Create<TriggerFullDateConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override TriggerFullDateCondition GetData(IQuery query)
        {
            List<TriggerFullDateConditionEntity> fullDateEntityList = dataAccess.GetList(query);
            if (fullDateEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = fullDateEntityList.First().TriggerId;
            TriggerFullDateCondition fullDateCondtion = new TriggerFullDateCondition(triggerId)
            {
                Dates = fullDateEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<FullDateConditionDate>()).ToList()
            };
            fullDateCondtion.MarkStored();
            return fullDateCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override List<TriggerFullDateCondition> GetDataList(IQuery query)
        {
            List<TriggerFullDateConditionEntity> fullDateEntityList = dataAccess.GetList(query);
            if (fullDateEntityList.IsNullOrEmpty())
            {
                return new List<TriggerFullDateCondition>(0);
            }
            IEnumerable<string> triggerIds = fullDateEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerFullDateCondition> fullDateConditions = new List<TriggerFullDateCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerFullDateCondition fullDateCondtion = new TriggerFullDateCondition(triggerId)
                {
                    Dates = fullDateEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<FullDateConditionDate>()).ToList()
                };
                fullDateCondtion.MarkStored();
                fullDateConditions.Add(fullDateCondtion);
            }
            return fullDateConditions;
        }

        #endregion

        #endregion
    }
}
