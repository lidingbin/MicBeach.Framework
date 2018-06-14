﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Query
{
    /// <summary>
    /// 错误日志信息
    /// </summary>
    public class ErrorLogDto
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 服务节点
        /// </summary>
        public ServerNodeDto Server
        {
            get;
            set;
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        public JobDto Job
        {
            get;
            set;
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        public TriggerDto Trigger
        {
            get;
            set;
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date
        {
            get;
            set;
        }

        #endregion

    }
}
