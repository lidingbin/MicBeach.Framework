using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 计划完整日期条件
    /// </summary>
    [Serializable]
    public class TriggerFullDateConditionEntity : CommandEntity<TriggerFullDateConditionEntity>
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
        /// 时间
        /// </summary>
        public DateTime Date
        {
            get { return valueDic.GetValue<DateTime>("Date"); }
            set { valueDic.SetValue("Date", value); }
        }

        /// <summary>
        /// 包含当前日期
        /// </summary>
        public bool Include
        {
            get { return valueDic.GetValue<bool>("Include"); }
            set { valueDic.SetValue("Include", value); }
        }

        #endregion
    }
}