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
using MicBeach.Query.Task;
using MicBeach.Domain.Task.Repository;
using MicBeach.Develop.Command;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 自定义附加计划存储
    /// </summary>
    public class TriggerExpressionConditionRepository : DefaultRepository<TriggerExpressionCondition, TriggerExpressionConditionEntity, ITriggerExpressionConditionDataAccess>, ITriggerExpressionConditionRepository
    {
        #region 重载功能

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="objDatas">要保存的数据</param>
        protected override void ExecuteSave(params TriggerExpressionCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = new List<TriggerExpressionConditionEntity>();
            foreach (var condition in objDatas)
            {
                if (condition == null || condition.ExpressionItems.IsNullOrEmpty())
                {
                    continue;
                }
                expressionConditionEntityList.AddRange(condition.ExpressionItems.Select(c => 
                {
                    var entity=c.MapTo<TriggerExpressionConditionEntity>();
                    entity.TriggerId = condition.TriggerId;
                    return entity;
                }).ToList());
            }
            //移除当前的条件
            List<string> triggerIds = objDatas.Select(c => c.TriggerId).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerExpressionConditionQuery>(c => triggerIds.Contains(c.TriggerId));
            Remove(removeQuery);
            //添加新的条件
            Add(expressionConditionEntityList.Distinct(new EntityCompare<TriggerExpressionConditionEntity>()).ToArray());
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="objDatas">要移除的数据</param>
        protected override void ExecuteRemove(params TriggerExpressionCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = new List<TriggerExpressionConditionEntity>();
            foreach (var obj in objDatas)
            {
                if (obj == null || obj.ExpressionItems.IsNullOrEmpty())
                {
                    continue;
                }
                expressionConditionEntityList.AddRange(obj.ExpressionItems.Select(c => 
                {
                    var entity=c.MapTo<TriggerExpressionConditionEntity>();
                    entity.TriggerId = obj.TriggerId;
                    return entity;
                }));
            }
            Remove(expressionConditionEntityList);
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
            Remove(QueryFactory.Create<TriggerExpressionConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override TriggerExpressionCondition GetData(IQuery query)
        {
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = dataAccess.GetList(query);
            if (expressionConditionEntityList.IsNullOrEmpty())
            {
                return null;
            }
            string triggerId = expressionConditionEntityList.First().TriggerId;
            TriggerExpressionCondition expressionCondtion = new TriggerExpressionCondition(triggerId)
            {
                ExpressionItems = expressionConditionEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<ExpressionItem>()).ToList()
            };
            expressionCondtion.MarkStored();
            return expressionCondtion;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        protected override List<TriggerExpressionCondition> GetDataList(IQuery query)
        {
            List<TriggerExpressionConditionEntity> expressionConditionEntityList = dataAccess.GetList(query);
            if (expressionConditionEntityList.IsNullOrEmpty())
            {
                return new List<TriggerExpressionCondition>(0);
            }
            IEnumerable<string> triggerIds = expressionConditionEntityList.Select(c => c.TriggerId).Distinct();
            List<TriggerExpressionCondition> expressionConditions = new List<TriggerExpressionCondition>();
            foreach (string triggerId in triggerIds)
            {
                TriggerExpressionCondition expressionCondtion = new TriggerExpressionCondition(triggerId)
                {
                    ExpressionItems = expressionConditionEntityList.Where(c => c.TriggerId == triggerId).Select(c => c.MapTo<ExpressionItem>()).ToList()
                };
                expressionCondtion.MarkStored();
                expressionConditions.Add(expressionCondtion);
            }
            return expressionConditions;
        }

        #endregion

        #endregion
    }
}
