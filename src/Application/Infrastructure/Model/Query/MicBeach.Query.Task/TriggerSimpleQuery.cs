using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 简单类型执行计划
    /// </summary>
    public class TriggerSimpleQuery : IQueryModel<TriggerSimpleQuery>
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
        /// 重复次数
        /// </summary>
        public int RepeatCount
        {
            get;
            set;
        }

        /// <summary>
        /// 重复间隔
        /// </summary>
        public long RepeatInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 一直重复执行
        /// </summary>
        public bool RepeatForever
        {
            get;
            set;
        }

        #endregion
    }
}