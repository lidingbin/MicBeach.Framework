using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 计划时间计划条件
    /// </summary>
    [Serializable]
    public class TriggerDailyConditionEntity : CommandEntity<TriggerDailyConditionEntity>
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
        /// 开始时间
        /// </summary>
        public string BeginTime
        {
            get { return valueDic.GetValue<string>("BeginTime"); }
            set { valueDic.SetValue("BeginTime", value); }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get { return valueDic.GetValue<string>("EndTime"); }
            set { valueDic.SetValue("EndTime", value); }
        }

        /// <summary>
        /// 启用设定值范围以外
        /// </summary>
        public bool Inversion
        {
            get { return valueDic.GetValue<bool>("Inversion"); }
            set { valueDic.SetValue("Inversion", value); }
        }

        #endregion
    }
}