using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Repository
{
    /// <summary>
    /// 计划条件存储
    /// </summary>
    public interface IBaseTriggerConditionRepository
    {
        #region 移除计划附加条件

        /// <summary>
        /// 移除计划附加条件
        /// </summary>
        /// <param name="triggers">执行计划</param>
        void RemoveTriggerConditionFromTrigger(IEnumerable<Trigger> triggers);

        #endregion
    }
}
