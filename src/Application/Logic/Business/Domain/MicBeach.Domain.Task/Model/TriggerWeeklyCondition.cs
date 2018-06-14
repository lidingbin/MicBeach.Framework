using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 星期条件
    /// </summary>
    public class TriggerWeeklyCondition : TriggerCondition
    {
        #region	字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<WeeklyConditionDay> _days;

        #endregion

        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<WeeklyConditionDay> Days
        {
            get
            {
                return _days;
            }
            set
            {
                _days = value;
            }
        }

        #endregion

        #region 构造方法

        public TriggerWeeklyCondition(string id = "") : base(id)
        {
            _type = TaskTriggerConditionType.星期配置;
        }

        #endregion
    }
}
