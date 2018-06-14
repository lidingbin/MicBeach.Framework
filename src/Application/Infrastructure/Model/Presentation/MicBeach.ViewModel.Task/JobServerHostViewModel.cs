using MicBeach.CTask;
using System;

namespace MicBeach.ViewModel.Task
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    public class JobServerHostViewModel
    {
        #region	属性

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNodeViewModel Server
        {
            get;
            set;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public JobViewModel Job
        {
            get;
            set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerState RunState
        {
            get;
            set;
        }

        #endregion
    }
}