using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 年计划天
    /// </summary>
    public class AnnualConditionDay
    {
        #region	字段

        /// <summary>
        /// 月份
        /// </summary>
        protected int _month;

        /// <summary>
        /// 日期
        /// </summary>
        protected int _day;

        /// <summary>
        /// 包含
        /// </summary>
        protected bool _include;

        #endregion

        #region	属性

        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get
            {
                return _month;
            }
            protected set
            {
                _month = value;
            }
        }

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
        /// 包含
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
