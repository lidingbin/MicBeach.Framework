using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 服务节点执行计划
    /// </summary>
    [Serializable]
    public class TriggerServerEntity : CommandEntity<TriggerServerEntity>
    {
        #region	字段

        /// <summary>
        /// 触发器
        /// </summary>
        public string Trigger
        {
            get { return valueDic.GetValue<string>("Trigger"); }
            set { valueDic.SetValue("Trigger", value); }
        }

        /// <summary>
        /// 服务
        /// </summary>
        public string Server
        {
            get { return valueDic.GetValue<string>("Server"); }
            set { valueDic.SetValue("Server", value); }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public int RunState
        {
            get { return valueDic.GetValue<int>("RunState"); }
            set { valueDic.SetValue("RunState", value); }
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime LastFireDate
        {
            get { return valueDic.GetValue<DateTime>("LastFireDate"); }
            set { valueDic.SetValue("LastFireDate", value); }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireDate
        {
            get { return valueDic.GetValue<DateTime>("NextFireDate"); }
            set { valueDic.SetValue("NextFireDate", value); }
        }

        #endregion
    }
}