using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 任务执行日志
    /// </summary>
    [Serializable]
    public class ExecuteLogEntity : CommandEntity<ExecuteLogEntity>
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
        /// 执行服务
        /// </summary>
        public string Server
        {
            get { return valueDic.GetValue<string>("Server"); }
            set { valueDic.SetValue("Server", value); }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get { return valueDic.GetValue<DateTime>("BeginTime"); }
            set { valueDic.SetValue("BeginTime", value); }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return valueDic.GetValue<DateTime>("EndTime"); }
            set { valueDic.SetValue("EndTime", value); }
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime
        {
            get { return valueDic.GetValue<DateTime>("RecordTime"); }
            set { valueDic.SetValue("RecordTime", value); }
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        public int State
        {
            get { return valueDic.GetValue<int>("State"); }
            set { valueDic.SetValue("State", value); }
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return valueDic.GetValue<string>("Message"); }
            set { valueDic.SetValue("Message", value); }
        }

        #endregion
    }
}