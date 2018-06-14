using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using MicBeach.Entity.Task;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Util.Extension;
using MicBeach.CTask;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Domain.Task.Repository;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 自定义表达式计划存储
    /// </summary>
    public class ExpressionTriggerRepository : DefaultRepository<ExpressionTrigger, TriggerExpressionEntity, ITriggerExpressionDataAccess>, IExpressionTriggerRepository
    {
        #region 接口方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void SaveExpressionTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<ExpressionTrigger> expressionTriggers = triggers.Where(c => c.Type == TaskTriggerType.自定义).Select(c => (ExpressionTrigger)c);
            if (!expressionTriggers.IsNullOrEmpty())
            {
                Save(expressionTriggers.ToArray());
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void RemoveExpressionTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<ExpressionTrigger> expressionTriggers = triggers.Where(c => c.Type == TaskTriggerType.自定义).Select(c => (ExpressionTrigger)c);
            if (expressionTriggers.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerExpressionEntity> expressionEntityList = new List<TriggerExpressionEntity>();
            foreach (var expressionTrigger in expressionTriggers)
            {
                if (expressionTrigger == null || expressionTrigger.ExpressionItems.IsNullOrEmpty())
                {
                    continue;
                }
                expressionEntityList.AddRange(expressionTrigger.ExpressionItems.Select(c =>
                {
                    var entity = c.MapTo<TriggerExpressionEntity>();
                    entity.TriggerId = expressionTrigger.Id;
                    return entity;
                }));
            }
            Remove(expressionEntityList);
        }

        /// <summary>
        /// 获取简单计划
        /// </summary>
        /// <param name="triggers">计划数据</param>
        /// <returns></returns>
        public void LoadExpressionTrigger(ref IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            List<Trigger> expressionTriggers = triggers.Where(c => c.Type == TaskTriggerType.自定义).ToList();
            IEnumerable<string> triggerIds = expressionTriggers.Select(c => c.Id);
            List<TriggerExpressionEntity> expressionTriggerDatas = dataAccess.GetList(QueryFactory.Create<TriggerExpressionQuery>(c => triggerIds.Contains(c.TriggerId)));
            List<Trigger> newExpressionTriggers = new List<Trigger>();
            foreach (var trigger in triggers)
            {
                if (trigger.Type != TaskTriggerType.自定义)
                {
                    newExpressionTriggers.Add(trigger);
                }
                else
                {

                    ExpressionTrigger nowExpressionTrigger = Trigger.CreateTrigger(trigger.Id, TaskTriggerType.自定义) as ExpressionTrigger;
                    nowExpressionTrigger.InitFromSimilarObject(trigger);//初始化信息
                    var nowExpressionDatas = expressionTriggerDatas.Where(s => s.TriggerId == trigger.Id);
                    if (!nowExpressionDatas.IsNullOrEmpty())
                    {
                        nowExpressionTrigger.ExpressionItems = nowExpressionDatas.Select(c => c.MapTo<ExpressionItem>()).ToList();
                    }
                    newExpressionTriggers.Add(nowExpressionTrigger);
                }
            }
            triggers = newExpressionTriggers;
        }

        #endregion

        #region 重载方法

        /// <summary>
        /// 保存计划表达式
        /// </summary>
        /// <param name="objDatas">要保持的数据</param>
        protected override void ExecuteSave(params ExpressionTrigger[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            List<TriggerExpressionEntity> expressionEntitys = new List<TriggerExpressionEntity>();
            List<string> triggerIds = new List<string>();
            foreach (var obj in objDatas)
            {
                triggerIds.Add(obj.Id);
                if (obj == null || obj.ExpressionItems.IsNullOrEmpty())
                {
                    continue;
                }
                expressionEntitys.AddRange(obj.ExpressionItems.Select(c =>
                {
                    var expression = c.MapTo<TriggerExpressionEntity>();
                    expression.TriggerId = obj.Id;
                    return expression;
                }));
            }
            //移除现有表达式
            IQuery removeQuery = QueryFactory.Create<TriggerExpressionQuery>(c => triggerIds.Contains(c.TriggerId));
            UnitOfWork.RegisterCommand(dataAccess.Delete(removeQuery));
            //保存
            UnitOfWork.RegisterCommand(dataAccess.Add(expressionEntitys).ToArray());
        }

        #endregion
    }
}
