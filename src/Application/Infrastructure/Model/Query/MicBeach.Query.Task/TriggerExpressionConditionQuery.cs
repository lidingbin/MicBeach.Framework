using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 计划表达式附加条件
    /// </summary>
    public class TriggerExpressionConditionQuery : IQueryModel<TriggerExpressionConditionQuery>
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
        /// 条件选项
        /// </summary>
        public int ConditionOption
        {
            get;
            set;
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public int ValueType
        {
            get;
            set;
        }

        /// <summary>
        /// 起始值
        /// </summary>
        public int BeginValue
        {
            get;
            set;
        }

        /// <summary>
        /// 结束值
        /// </summary>
        public int EndValue
        {
            get;
            set;
        }

        /// <summary>
        /// 集合值
        /// </summary>
        public string ArrayValue
        {
            get;
            set;
        }

        #endregion
    }
}