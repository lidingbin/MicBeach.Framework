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
using MicBeach.Develop.CQuery;
using MicBeach.Domain.Task.Repository;
using MicBeach.Query.Task;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 每日时间段计划存储
    /// </summary>
    public class TriggerDailyConditionRepository : DefaultRepository<TriggerDailyCondition, TriggerDailyConditionEntity, ITriggerDailyConditionDataAccess>, ITriggerDailyConditionRepository
    {
        #region 保存

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="objDatas">条件数据</param>
        protected override void ExecuteSave(params TriggerDailyCondition[] objDatas)
        {
            if (objDatas.IsNullOrEmpty())
            {
                return;
            }
            var triggerIds = objDatas.Select(c => c.TriggerId).Distinct().ToList();
            //移除当前的条件信息
            Remove(QueryFactory.Create<TriggerDailyConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
            base.ExecuteSave(objDatas);
        }

        #endregion

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
            Remove(QueryFactory.Create<TriggerDailyConditionQuery>(c => triggerIds.Contains(c.TriggerId)));
        }
    }
}
