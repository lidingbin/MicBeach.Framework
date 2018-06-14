using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 任务执行计划
    /// </summary>
    [Serializable]
    public class TriggerEntity : CommandEntity<TriggerEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get { return valueDic.GetValue<string>("Id"); }
            set { valueDic.SetValue("Id", value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return valueDic.GetValue<string>("Name"); }
            set { valueDic.SetValue("Name", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get { return valueDic.GetValue<string>("Description"); }
            set { valueDic.SetValue("Description", value); }
        }

        /// <summary>
        /// 所属任务
        /// </summary>
        public string Job
        {
            get { return valueDic.GetValue<string>("Job"); }
            set { valueDic.SetValue("Job", value); }
        }

        /// <summary>
        /// 应用到对象
        /// </summary>
        public int ApplyTo
        {
            get { return valueDic.GetValue<int>("ApplyTo"); }
            set { valueDic.SetValue("ApplyTo", value); }
        }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime PrevFireTime
        {
            get { return valueDic.GetValue<DateTime>("PrevFireTime"); }
            set { valueDic.SetValue("PrevFireTime", value); }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireTime
        {
            get { return valueDic.GetValue<DateTime>("NextFireTime"); }
            set { valueDic.SetValue("NextFireTime", value); }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get { return valueDic.GetValue<int>("Priority"); }
            set { valueDic.SetValue("Priority", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public int State
        {
            get { return valueDic.GetValue<int>("State"); }
            set { valueDic.SetValue("State", value); }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type
        {
            get { return valueDic.GetValue<int>("Type"); }
            set { valueDic.SetValue("Type", value); }
        }

        /// <summary>
        /// 额外条件类型
        /// </summary>
        public int ConditionType
        {
            get { return valueDic.GetValue<int>("ConditionType"); }
            set { valueDic.SetValue("ConditionType", value); }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return valueDic.GetValue<DateTime>("StartTime"); }
            set { valueDic.SetValue("StartTime", value); }
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
        /// 执行失败操作类型
        /// </summary>
        public int MisFireType
        {
            get { return valueDic.GetValue<int>("MisFireType"); }
            set { valueDic.SetValue("MisFireType", value); }
        }

        /// <summary>
        /// 总触发次数
        /// </summary>
        public int FireTotalCount
        {
            get { return valueDic.GetValue<int>("FireTotalCount"); }
            set { valueDic.SetValue("FireTotalCount", value); }
        }

        #endregion
    }
}