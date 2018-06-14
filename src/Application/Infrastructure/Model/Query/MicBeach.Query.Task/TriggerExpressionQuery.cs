﻿using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 自定义表达式计划
    /// </summary>
    public class TriggerExpressionQuery : IQueryModel<TriggerExpressionQuery>
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
        /// 表达式配置项
        /// </summary>
        public int Option
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
        /// 开始值
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