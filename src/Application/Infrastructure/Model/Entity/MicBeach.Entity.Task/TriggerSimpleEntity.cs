using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 简单类型执行计划
    /// </summary>
    [Serializable]
    public class TriggerSimpleEntity : CommandEntity<TriggerSimpleEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        public string TriggerId
        {
            get { return valueDic.GetValue<string>("TriggerId"); }
            set { valueDic.SetValue("TriggerId", value); }
        }

        /// <summary>
        /// 重复次数
        /// </summary>
        public int RepeatCount
        {
            get { return valueDic.GetValue<int>("RepeatCount"); }
            set { valueDic.SetValue("RepeatCount", value); }
        }

        /// <summary>
        /// 重复间隔
        /// </summary>
        public long RepeatInterval
        {
            get { return valueDic.GetValue<long>("RepeatInterval"); }
            set { valueDic.SetValue("RepeatInterval", value); }
        }

        /// <summary>
        /// 一直重复执行
        /// </summary>
        public bool RepeatForever
        {
            get { return valueDic.GetValue<bool>("RepeatForever"); }
            set { valueDic.SetValue("RepeatForever", value); }
        }

        #endregion
    }
}