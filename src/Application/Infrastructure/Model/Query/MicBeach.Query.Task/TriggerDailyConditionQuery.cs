using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 计划时间计划条件
    /// </summary>
    public class TriggerDailyConditionQuery : IQueryModel<TriggerDailyConditionQuery>
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
        /// 开始时间
        /// </summary>
        public string BeginTime
        {
            get;
            set;
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 启用设定值范围以外
        /// </summary>
        public bool Inversion
        {
            get;
            set;
        }

        #endregion
    }
}