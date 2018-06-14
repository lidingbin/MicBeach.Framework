using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 月份附加条件
    /// </summary>
    public class TriggerMonthlyCondition : TriggerCondition
    {
        #region	字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<MonthConditionDay> _days;

        #endregion

        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<MonthConditionDay> Days
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

        public TriggerMonthlyCondition(string id = ""):base(id)
        {
            _type = TaskTriggerConditionType.每月日期;
        }

        #endregion
    }
}
