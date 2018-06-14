﻿using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 服务节点执行计划
    /// </summary>
    public class TriggerServerQuery : IQueryModel<TriggerServerQuery>
    {
        #region	属性

        /// <summary>
        /// 触发器
        /// </summary>
        public string Trigger
        {
            get;
            set;
        }

        /// <summary>
        /// 服务
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int RunState
        {
            get;
            set;
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime LastFireDate
        {
            get;
            set;
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireDate
        {
            get;
            set;
        }

        #endregion
    }
}