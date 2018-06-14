using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 计划年度条件
    /// </summary>
    public class TriggerAnnualConditionQuery : IQueryModel<TriggerAnnualConditionQuery>
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string TriggerId
        {
            get;
            set;
        }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month
        {
            get;
            set;
        }

        /// <summary>
        /// 日期
        /// </summary>
        public int Day
        {
            get;
            set;
        }

        /// <summary>
        /// 包含
        /// </summary>
        public bool Include
        {
            get;
            set;
        }

        #endregion
    }
}