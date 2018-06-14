using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 日期
    /// </summary>
    public class FullDateConditionDate
    {
        #region	字段

        /// <summary>
        /// 时间
        /// </summary>
        protected DateTime _date;

        /// <summary>
        /// 包含当前日期
        /// </summary>
        protected bool _include;

        #endregion

        #region	属性

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _date;
            }
            protected set
            {
                _date = value;
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
