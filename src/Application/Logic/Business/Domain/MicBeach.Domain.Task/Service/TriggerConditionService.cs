using MicBeach.CTask;
using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Extension;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.IoC;

namespace MicBeach.Domain.Task.Service
{
    /// <summary>
    /// 计划条件服务
    /// </summary>
    public static class TriggerConditionService
    {
        static ITriggerConditionRepository conditionRepository = ContainerManager.Resolve<ITriggerConditionRepository>();

        #region 获取指定执行计划的条件

        /// <summary>
        /// 获取指定计划的附加条件
        /// </summary>
        /// <param name="triggerId">计划编号</param>
        /// <param name="conditionType">条件类型</param>
        /// <returns></returns>
        public static TriggerCondition GetTriggerConditionByTrigger(string triggerId, TaskTriggerConditionType conditionType)
        {
            if (triggerId.IsNullOrEmpty())
            {
                return null;
            }
            return conditionRepository.GetTriggerCondition(triggerId, conditionType);
        }

        /// <summary>
        /// 获取执行计划的附加条件
        /// </summary>
        /// <param name="triggers">执行计划信息</param>
        /// <returns></returns>
        public static List<TriggerCondition> GetTriggerConditionList(IEnumerable<Trigger> triggers)
        {
            if (triggers.IsNullOrEmpty())
            {
                return new List<TriggerCondition>(0);
            }
            return conditionRepository.GetTriggerConditionList(triggers);
        }

        #endregion
    }
}
