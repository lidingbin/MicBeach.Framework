using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 任务服务承载信息
    /// </summary>
    public class JobServerHostCmdDto
    {
        #region	属性

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNodeCmdDto Server
        {
            get;
            set;
        }

        /// <summary>
        /// 任务
        /// </summary>
        public JobCmdDto Job
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
