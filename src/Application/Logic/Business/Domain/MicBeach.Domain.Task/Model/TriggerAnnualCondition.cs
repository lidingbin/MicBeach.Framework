using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 触发条件
    /// </summary>
    public class TriggerAnnualCondition : TriggerCondition
    {
        #region 字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<AnnualConditionDay> _days;

        #endregion

        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<AnnualConditionDay> Days
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

        public TriggerAnnualCondition(string id = "") : base(id)
        {
            _type = TaskTriggerConditionType.每年日期;
        }

        #endregion
    }
}
