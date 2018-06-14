using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 自定义表达式计划
    /// </summary>
    [Serializable]
    public class TriggerExpressionEntity : CommandEntity<TriggerExpressionEntity>
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
        /// 表达式配置项
        /// </summary>
        public int Option
        {
            get { return valueDic.GetValue<int>("Option"); }
            set { valueDic.SetValue("Option", value); }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public int ValueType
        {
            get { return valueDic.GetValue<int>("ValueType"); }
            set { valueDic.SetValue("ValueType", value); }
        }

        /// <summary>
        /// 开始值
        /// </summary>
        public int BeginValue
        {
            get { return valueDic.GetValue<int>("BeginValue"); }
            set { valueDic.SetValue("BeginValue", value); }
        }

        /// <summary>
        /// 结束值
        /// </summary>
        public int EndValue
        {
            get { return valueDic.GetValue<int>("EndValue"); }
            set { valueDic.SetValue("EndValue", value); }
        }

        /// <summary>
        /// 集合值
        /// </summary>
        public string ArrayValue
        {
            get { return valueDic.GetValue<string>("ArrayValue"); }
            set { valueDic.SetValue("ArrayValue", value); }
        }

        #endregion
    }
}