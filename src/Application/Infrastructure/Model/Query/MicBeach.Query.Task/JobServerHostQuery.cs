using System;
using System.Collections.Generic;
using MicBeach.Develop.CQuery;

namespace MicBeach.Query.Task
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    public class JobServerHostQuery : IQueryModel<JobServerHostQuery>
    {
        #region	属性

        /// <summary>
        /// 服务
        /// </summary>
        public string Server
        {
            get;
            set;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public string Job
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

        #endregion
    }
}