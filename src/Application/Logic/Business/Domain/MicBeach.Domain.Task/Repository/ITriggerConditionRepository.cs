using MicBeach.CTask;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Repository
{
    /// <summary>
    /// 计划附加条件存储接口
    /// </summary>
    public interface ITriggerConditionRepository : IBaseTriggerConditionRepository
    {

        #region 属性

        /// <summary>
        /// 数据移除事件
        /// </summary>
        event DataChange<Trigger> RemoveFromTriggerEvent;

        #endregion

        #region 获取指定计划的附加条件

        /// <summary>
        /// 获取指定计划的附加条件
        /// </summary>
        /// <param name="triggerId">计划编号</param>
        /// <param name="conditionType">计划类型</param>
        /// <returns></returns>
        TriggerCondition GetTriggerCondition(string triggerId, TaskTriggerConditionType conditionType);

        /// <summary>
        /// 获取执行计划附加条件
        /// </summary>
        /// <param name="triggers">执行计划信息</param>
        /// <returns></returns>
        List<TriggerCondition> GetTriggerConditionList(IEnumerable<Trigger> triggers);

        #endregion

        #region 保存计划附加条件

        /// <summary>
        /// 保存计划附加条件
        /// </summary>
        /// <param name="triggerConditions">附加条件信息</param>
        void SaveTriggerCondition(IEnumerable<TriggerCondition> triggerConditions);

        /// <summary>
        /// 保存执行计划下的附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        void SaveTriggerConditionFromTrigger(IEnumerable<Trigger> triggers);

        #endregion

        #region 移除计划附加条件

        /// <summary>
        /// 移除计划附加条件
        /// </summary>
        /// <param name="triggerConditions">附加条件</param>
        void RemoveTriggerCondition(IEnumerable<TriggerCondition> triggerConditions);

        #endregion
    }
}
