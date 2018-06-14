using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    [Serializable]
    public class JobServerHostEntity : CommandEntity<JobServerHostEntity>
    {
        #region	字段

        /// <summary>
        /// 服务
        /// </summary>
        public string Server
        {
            get { return valueDic.GetValue<string>("Server"); }
            set { valueDic.SetValue("Server", value); }
        }

        /// <summary>
        /// 任务
        /// </summary>
        public string Job
        {
            get { return valueDic.GetValue<string>("Job"); }
            set { valueDic.SetValue("Job", value); }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int RunState
        {
            get { return valueDic.GetValue<int>("RunState"); }
            set { valueDic.SetValue("RunState", value); }
        }

        #endregion
    }
}