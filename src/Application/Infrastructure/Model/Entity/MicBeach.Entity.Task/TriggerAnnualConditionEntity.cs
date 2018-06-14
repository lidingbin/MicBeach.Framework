using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 计划年度条件
    /// </summary>
    [Serializable]
    public class TriggerAnnualConditionEntity : CommandEntity<TriggerAnnualConditionEntity>
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
        /// 月份
        /// </summary>
        public int Month
        {
            get { return valueDic.GetValue<int>("Month"); }
            set { valueDic.SetValue("Month", value); }
        }

        /// <summary>
        /// 日期
        /// </summary>
        public int Day
        {
            get { return valueDic.GetValue<int>("Day"); }
            set { valueDic.SetValue("Day", value); }
        }

        /// <summary>
        /// 包含
        /// </summary>
        public bool Include
        {
            get { return valueDic.GetValue<bool>("Include"); }
            set { valueDic.SetValue("Include", value); }
        }

        #endregion
    }
}