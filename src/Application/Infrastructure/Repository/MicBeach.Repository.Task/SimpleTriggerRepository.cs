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
using MicBeach.CTask;
using MicBeach.Develop.CQuery;
using MicBeach.Domain.Task.Repository;
using MicBeach.Query.Task;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 简单计划存储
    /// </summary>
    public class SimpleTriggerRepository : DefaultRepository<SimpleTrigger, TriggerSimpleEntity, ITriggerSimpleDataAccess>, ISimpleTriggerRepository
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void SaveSimpleTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<SimpleTrigger> simpleTriggers = triggers.Where(c => c.Type == TaskTriggerType.简单).Select(c => (SimpleTrigger)c);
            if (!simpleTriggers.IsNullOrEmpty())
            {
                Save(simpleTriggers.ToArray());
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="triggers">计划数据</param>
        public void RemoveSimpleTrigger(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<SimpleTrigger> simpleTriggers = triggers.Where(c => c.Type == TaskTriggerType.简单).Select(c => (SimpleTrigger)c);
            if (simpleTriggers.IsNullOrEmpty())
            {
                return;
            }
            Remove(simpleTriggers.Select(c =>
            {
                return new TriggerSimpleEntity()
                {
                    TriggerId = c.Id,
                    RepeatCount = c.RepeatCount,
                    RepeatForever = c.RepeatForever,
                    RepeatInterval = c.RepeatInterval
                };
            }).ToArray());
        }

        /// <summary>
        /// 获取简单计划
        /// </summary>
        /// <param name="triggers">计划数据</param>
        /// <returns></returns>
        public void LoadSimpleTrigger(ref IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return;
            }
            List<Trigger> simpleTriggers = triggers.Where(c => c.Type == TaskTriggerType.简单).ToList();
            IEnumerable<string> triggerIds = simpleTriggers.Select(c => c.Id);
            List<TriggerSimpleEntity> simpleTriggerDatas = dataAccess.GetList(QueryFactory.Create<TriggerSimpleQuery>(c => triggerIds.Contains(c.TriggerId)));
            List<Trigger> newSimpleTriggers = new List<Trigger>();
            foreach (var trigger in triggers)
            {
                if (trigger.Type != TaskTriggerType.简单)
                {
                    newSimpleTriggers.Add(trigger);
                }
                else
                {
                    SimpleTrigger nowSimpleTrigger = null;
                    var nowSimpleData = simpleTriggerDatas.FirstOrDefault(s => s.TriggerId == trigger.Id);
                    if (nowSimpleData != null)
                    {
                        nowSimpleTrigger = nowSimpleData.MapTo<SimpleTrigger>();
                    }
                    else
                    {
                        nowSimpleTrigger = Trigger.CreateTrigger(id:trigger.Id) as SimpleTrigger;
                    }
                    nowSimpleTrigger.InitFromSimilarObject(trigger);
                    newSimpleTriggers.Add(nowSimpleTrigger);
                }
            }
            triggers = newSimpleTriggers;
        }
    }
}
