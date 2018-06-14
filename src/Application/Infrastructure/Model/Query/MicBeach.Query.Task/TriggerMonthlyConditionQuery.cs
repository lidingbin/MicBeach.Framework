using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 计划月份附加条件
    /// </summary>
    public class TriggerMonthlyConditionQuery : IQueryModel<TriggerMonthlyConditionQuery>
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
        /// 日期
        /// </summary>
        public int Day
        {
            get;
            set;
        }

        /// <summary>
        /// 包含当前日期
        /// </summary>
        public bool Include
        {
            get;
            set;
        }

        #endregion
    }
}