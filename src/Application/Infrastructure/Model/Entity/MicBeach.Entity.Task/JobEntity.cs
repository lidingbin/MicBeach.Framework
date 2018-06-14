using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 工作任务
    /// </summary>
    [Serializable]
    public class JobEntity : CommandEntity<JobEntity>
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
        /// 分组
        /// </summary>
        public string Group
        {
            get { return valueDic.GetValue<string>("Group"); }
            set { valueDic.SetValue("Group", value); }
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
        /// 任务类型
        /// </summary>
        public int Type
        {
            get { return valueDic.GetValue<int>("Type"); }
            set { valueDic.SetValue("Type", value); }
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        public int RunType
        {
            get { return valueDic.GetValue<int>("RunType"); }
            set { valueDic.SetValue("RunType", value); }
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
        /// 说明
        /// </summary>
        public string Description
        {
            get { return valueDic.GetValue<string>("Description"); }
            set { valueDic.SetValue("Description", value); }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate
        {
            get { return valueDic.GetValue<DateTime>("UpdateDate"); }
            set { valueDic.SetValue("UpdateDate", value); }
        }

        /// <summary>
        /// 任务路径
        /// </summary>
        public string JobPath
        {
            get { return valueDic.GetValue<string>("JobPath"); }
            set { valueDic.SetValue("JobPath", value); }
        }

        /// <summary>
        /// 任务文件名称
        /// </summary>
        public string JobFileName
        {
            get { return valueDic.GetValue<string>("JobFileName"); }
            set { valueDic.SetValue("JobFileName", value); }
        }

        #endregion
    }
}