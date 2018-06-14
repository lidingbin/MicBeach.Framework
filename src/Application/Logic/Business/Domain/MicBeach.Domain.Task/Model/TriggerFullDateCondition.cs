using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 时间条件
    /// </summary>
    public class TriggerFullDateCondition:TriggerCondition
    {
        #region 字段

        /// <summary>
        /// 日期
        /// </summary>
        protected List<FullDateConditionDate> _dates;

        #endregion

        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<FullDateConditionDate> Dates
        {
            get
            {
                return _dates;
            }
            set
            {
                _dates = value;
            }
        }

        #endregion

        #region 构造方法

        public TriggerFullDateCondition(string id = "") : base(id)
        {
            _type = TaskTriggerConditionType.固定日期;
        }

        #endregion
    }
}
