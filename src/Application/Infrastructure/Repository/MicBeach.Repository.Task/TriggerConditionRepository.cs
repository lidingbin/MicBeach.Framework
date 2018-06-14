using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Entity.Task;
using MicBeach.Develop.CQuery;
using MicBeach.Domain.Task.Repository;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.CTask;
using MicBeach.Query.Task;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 计划附加条件存储
    /// </summary>
    public class TriggerConditionRepository : ITriggerConditionRepository
    {
        ITriggerFullDateConditionRepository fullDateConditionRepository = null;//完整日期计划数据存储
        ITriggerAnnualConditionRepository annualConditionRepository = null;//年度计划数据存储
        ITriggerDailyConditionRepository dailyConditionRepository = null;//时间段计划存储
        ITriggerExpressionConditionRepository expressionConditionRepository = null;//自定义计划存储
        ITriggerMonthlyConditionRepository monthlyConditionRepository = null;//每月日期存储
        ITriggerWeeklyConditionRepository weeklyConditionRepository = null;//星期计划存储
        List<IBaseTriggerConditionRepository> triggerRepositorys = null;
        public TriggerConditionRepository()
        {
            fullDateConditionRepository = this.Instance<ITriggerFullDateConditionRepository>();
            annualConditionRepository = this.Instance<ITriggerAnnualConditionRepository>();
            dailyConditionRepository = this.Instance<ITriggerDailyConditionRepository>();
            expressionConditionRepository = this.Instance<ITriggerExpressionConditionRepository>();
            monthlyConditionRepository = this.Instance<ITriggerMonthlyConditionRepository>();
            weeklyConditionRepository = this.Instance<ITriggerWeeklyConditionRepository>();
            triggerRepositorys = new List<IBaseTriggerConditionRepository>()
            {
                fullDateConditionRepository,annualConditionRepository,dailyConditionRepository,expressionConditionRepository,monthlyConditionRepository,weeklyConditionRepository
            };
            BindEvent();
        }

        #region 属性

        /// <summary>
        /// 根据计划移除条件事件
        /// </summary>

        public event DataChange<Trigger> RemoveFromTriggerEvent;

        #endregion

        #region 获取指定计划的附加条件

        /// <summary>
        /// 获取指定计划的附加条件
        /// </summary>
        /// <param name="triggerId">计划编号</param>
        /// <param name="conditionType">条件类型</param>
        /// <returns></returns>
        public TriggerCondition GetTriggerCondition(string triggerId, TaskTriggerConditionType conditionType)
        {
            if (triggerId.IsNullOrEmpty())
            {
                return null;
            }
            var trigger = Trigger.CreateTrigger(triggerId);
            trigger.SetCondition(TriggerCondition.CreateTriggerCondition(conditionType, triggerId), true);
            var conditions = GetTriggerConditionList(new List<Trigger>()
            {
                trigger
            });
            if (conditions.IsNullOrEmpty())
            {
                return null;
            }
            return conditions.FirstOrDefault();
        }

        /// <summary>
        /// 获取执行计划附加条件
        /// </summary>
        /// <param name="triggers">执行计划信息</param>
        /// <returns></returns>
        public List<TriggerCondition> GetTriggerConditionList(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            Dictionary<TaskTriggerConditionType, List<string>> conditionGroups = new Dictionary<TaskTriggerConditionType, List<string>>();
            foreach (var trigger in triggers)
            {
                if (trigger == null || trigger.Condition == null)
                {
                    continue;
                }
                if (conditionGroups.ContainsKey(trigger.Condition.Type))
                {
                    conditionGroups[trigger.Condition.Type].Add(trigger.Id);
                }
                else
                {
                    conditionGroups.Add(trigger.Condition.Type, new List<string>() { trigger.Id });
                }
            }
            List<TriggerCondition> conditons = new List<TriggerCondition>();
            foreach (var conditionGroupItem in conditionGroups)
            {
                switch (conditionGroupItem.Key)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        conditons.AddRange(GetFullDateCondition(conditionGroupItem.Value));
                        break;
                    case TaskTriggerConditionType.星期配置:
                        conditons.AddRange(GetWeeklyCondition(conditionGroupItem.Value));
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        conditons.AddRange(GetDailyCondition(conditionGroupItem.Value));
                        break;
                    case TaskTriggerConditionType.每年日期:
                        conditons.AddRange(GetAnnualCondition(conditionGroupItem.Value));
                        break;
                    case TaskTriggerConditionType.每月日期:
                        conditons.AddRange(GetMonthlyCondition(conditionGroupItem.Value));
                        break;
                    case TaskTriggerConditionType.自定义:
                        conditons.AddRange(GetExpressionCondition(conditionGroupItem.Value));
                        break;
                }
            }
            return conditons;
        }

        /// <summary>
        /// 获取年度附加条件
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        List<TriggerAnnualCondition> GetAnnualCondition(IEnumerable<string> triggerIds)
        {
            if (triggerIds.IsNullOrEmpty())
            {
                return null;
            }
            return annualConditionRepository.GetList(QueryFactory.Create<TriggerAnnualConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        /// <summary>
        /// 获取每天时间段附加条件
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        List<TriggerDailyCondition> GetDailyCondition(IEnumerable<string> triggerIds)
        {
            if (triggerIds.IsNullOrEmpty())
            {
                return null;
            }
            return dailyConditionRepository.GetList(QueryFactory.Create<TriggerDailyConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        /// <summary>
        /// 获取自定义附加条件
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        List<TriggerExpressionCondition> GetExpressionCondition(IEnumerable<string> triggerIds)
        {
            if (triggerIds.IsNullOrEmpty())
            {
                return null;
            }
            return expressionConditionRepository.GetList(QueryFactory.Create<TriggerExpressionConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        /// <summary>
        /// 获取完整日期附加条件
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        List<TriggerFullDateCondition> GetFullDateCondition(IEnumerable<string> triggerIds)
        {
            if (triggerIds.IsNullOrEmpty())
            {
                return null;
            }
            return fullDateConditionRepository.GetList(QueryFactory.Create<TriggerFullDateConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        /// <summary>
        /// 获取月份附加条件
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        List<TriggerMonthlyCondition> GetMonthlyCondition(IEnumerable<string> triggerIds)
        {
            if (triggerIds.IsNullOrEmpty())
            {
                return null;
            }
            return monthlyConditionRepository.GetList(QueryFactory.Create<TriggerMonthlyConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        /// <summary>
        /// 获取星期附加条件
        /// </summary>
        /// <param name="triggerIds">计划编号</param>
        /// <returns></returns>
        List<TriggerWeeklyCondition> GetWeeklyCondition(IEnumerable<string> triggerIds)
        {
            if (triggerIds.IsNullOrEmpty())
            {
                return null;
            }
            return weeklyConditionRepository.GetList(QueryFactory.Create<TriggerWeeklyConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }

        #endregion

        #region 保存计划附加条件

        /// <summary>
        /// 保存计划附加条件
        /// </summary>
        /// <param name="triggerConditions">附加条件信息</param>
        public void SaveTriggerCondition(IEnumerable<TriggerCondition> triggerConditions)
        {
            if (triggerConditions.IsNullOrEmpty())
            {
                return;
            }
            foreach (var triggerConditon in triggerConditions)
            {
                if (triggerConditon == null)
                {
                    continue;
                }
                switch (triggerConditon.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        fullDateConditionRepository.Save(triggerConditon as TriggerFullDateCondition);
                        break;
                    case TaskTriggerConditionType.星期配置:
                        weeklyConditionRepository.Save(triggerConditon as TriggerWeeklyCondition);
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        dailyConditionRepository.Save(triggerConditon as TriggerDailyCondition);
                        break;
                    case TaskTriggerConditionType.每年日期:
                        annualConditionRepository.Save(triggerConditon as TriggerAnnualCondition);
                        break;
                    case TaskTriggerConditionType.每月日期:
                        monthlyConditionRepository.Save(triggerConditon as TriggerMonthlyCondition);
                        break;
                    case TaskTriggerConditionType.自定义:
                        expressionConditionRepository.Save(triggerConditon as TriggerExpressionCondition);
                        break;

                }
            }
        }

        /// <summary>
        /// 保存执行计划下的附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        public void SaveTriggerConditionFromTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<TriggerCondition> conditions = triggers.Select(c => c?.Condition);
            SaveTriggerCondition(conditions);
        }

        #endregion

        #region 移除计划附加条件

        /// <summary>
        /// 移除计划附加条件
        /// </summary>
        /// <param name="triggerConditions">附加条件</param>
        public void RemoveTriggerCondition(IEnumerable<TriggerCondition> triggerConditions)
        {
            if (triggerConditions.IsNullOrEmpty())
            {
                return;
            }
            foreach (var triggerConditon in triggerConditions)
            {
                if (triggerConditon == null)
                {
                    continue;
                }
                switch (triggerConditon.Type)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        fullDateConditionRepository.Remove(triggerConditon as TriggerFullDateCondition);
                        break;
                    case TaskTriggerConditionType.星期配置:
                        weeklyConditionRepository.Remove(triggerConditon as TriggerWeeklyCondition);
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        dailyConditionRepository.Remove(triggerConditon as TriggerDailyCondition);
                        break;
                    case TaskTriggerConditionType.每年日期:
                        annualConditionRepository.Remove(triggerConditon as TriggerAnnualCondition);
                        break;
                    case TaskTriggerConditionType.每月日期:
                        monthlyConditionRepository.Remove(triggerConditon as TriggerMonthlyCondition);
                        break;
                    case TaskTriggerConditionType.自定义:
                        expressionConditionRepository.Remove(triggerConditon as TriggerExpressionCondition);
                        break;

                }
            }
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
            List<Trigger> nullConditionTriggers = new List<Trigger>();
            Dictionary<TaskTriggerConditionType, List<Trigger>> typeTriggers = new Dictionary<TaskTriggerConditionType, List<Trigger>>();
            foreach (var trigger in triggers)
            {
                if (trigger == null)
                {
                    continue;
                }
                if (trigger.Condition == null)
                {
                    nullConditionTriggers.Add(trigger);
                    continue;
                }
                if (typeTriggers.ContainsKey(trigger.Condition.Type))
                {
                    typeTriggers[trigger.Condition.Type].Add(trigger);
                }
                else
                {
                    typeTriggers.Add(trigger.Condition.Type, new List<Trigger>()
                    {
                        trigger
                    });
                }
            }
            foreach (var typeTriggerItem in typeTriggers)
            {
                switch (typeTriggerItem.Key)
                {
                    case TaskTriggerConditionType.不限制:
                        break;
                    case TaskTriggerConditionType.固定日期:
                        fullDateConditionRepository.RemoveTriggerConditionFromTrigger(typeTriggerItem.Value);
                        break;
                    case TaskTriggerConditionType.星期配置:
                        weeklyConditionRepository.RemoveTriggerConditionFromTrigger(typeTriggerItem.Value);
                        break;
                    case TaskTriggerConditionType.每天时间段:
                        dailyConditionRepository.RemoveTriggerConditionFromTrigger(typeTriggerItem.Value);
                        break;
                    case TaskTriggerConditionType.每年日期:
                        annualConditionRepository.RemoveTriggerConditionFromTrigger(typeTriggerItem.Value);
                        break;
                    case TaskTriggerConditionType.每月日期:
                        monthlyConditionRepository.RemoveTriggerConditionFromTrigger(typeTriggerItem.Value);
                        break;
                    case TaskTriggerConditionType.自定义:
                        expressionConditionRepository.RemoveTriggerConditionFromTrigger(typeTriggerItem.Value);
                        break;
                }
            }
            if (!nullConditionTriggers.IsNullOrEmpty())
            {
                RemoveFromTriggerEvent?.Invoke(triggers.ToArray());
            }
        }

        #endregion

        #region 绑定事件

        /// <summary>
        /// 绑定事件
        /// </summary>
        void BindEvent()
        {
            foreach (var conditionRepository in triggerRepositorys)
            {
                RemoveFromTriggerEvent += conditionRepository.RemoveTriggerConditionFromTrigger;
            }
        }

        #endregion
    }
}
