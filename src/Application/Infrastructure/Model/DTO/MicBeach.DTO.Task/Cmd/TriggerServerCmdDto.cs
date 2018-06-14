﻿using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 计划服务信息
    /// </summary>
    public class TriggerServerCmdDto
    {
        #region	属性

        /// <summary>
        /// 触发器
        /// </summary>
        public TriggerCmdDto Trigger
        {
            get;
            set;
        }

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNodeCmdDto Server
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public TaskTriggerServerRunState RunState
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
