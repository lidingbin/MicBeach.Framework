using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 任务异常日志
    /// </summary>
    [Serializable]
    public class ErrorLogEntity : CommandEntity<ErrorLogEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        public long Id
        {
            get { return valueDic.GetValue<long>("Id"); }
            set { valueDic.SetValue("Id", value); }
        }

        /// <summary>
        /// 服务节点
        /// </summary>
        public string Server
        {
            get { return valueDic.GetValue<string>("Server"); }
            set { valueDic.SetValue("Server", value); }
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        public string Job
        {
            get { return valueDic.GetValue<string>("Job"); }
            set { valueDic.SetValue("Job", value); }
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        public string Trigger
        {
            get { return valueDic.GetValue<string>("Trigger"); }
            set { valueDic.SetValue("Trigger", value); }
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message
        {
            get { return valueDic.GetValue<string>("Message"); }
            set { valueDic.SetValue("Message", value); }
        }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description
        {
            get { return valueDic.GetValue<string>("Description"); }
            set { valueDic.SetValue("Description", value); }
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        public int Type
        {
            get { return valueDic.GetValue<int>("Type"); }
            set { valueDic.SetValue("Type", value); }
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date
        {
            get { return valueDic.GetValue<DateTime>("Date"); }
            set { valueDic.SetValue("Date", value); }
        }

        #endregion
    }
}