using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 计划月份附加条件
    /// </summary>
    [Serializable]
    public class TriggerMonthlyConditionEntity : CommandEntity<TriggerMonthlyConditionEntity>
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
        /// 日期
        /// </summary>
        public int Day
        {
            get { return valueDic.GetValue<int>("Day"); }
            set { valueDic.SetValue("Day", value); }
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