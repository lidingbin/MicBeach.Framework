using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 月计划日期
    /// </summary>
    public class MonthConditionDay
    {
        #region	字段

        /// <summary>
        /// 日期
        /// </summary>
        protected int _day;

        /// <summary>
        /// 包含当前日期
        /// </summary>
        protected bool _include;

        #endregion

        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public int Day
        {
            get
            {
                return _day;
            }
            protected set
            {
                _day = value;
            }
        }

        /// <summary>
        /// 包含当前日期
        /// </summary>
        public bool Include
        {
            get
            {
                return _include;
            }
            protected set
            {
                _include = value;
            }
        }

        #endregion
    }
}
