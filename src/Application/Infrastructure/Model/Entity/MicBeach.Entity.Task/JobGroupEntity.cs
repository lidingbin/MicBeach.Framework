using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 工作分组
    /// </summary>
    [Serializable]
    public class JobGroupEntity : CommandEntity<JobGroupEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        public string Code
        {
            get { return valueDic.GetValue<string>("Code"); }
            set { valueDic.SetValue("Code", value); }
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
        /// 排序
        /// </summary>
        public int Sort
        {
            get { return valueDic.GetValue<int>("Sort"); }
            set { valueDic.SetValue("Sort", value); }
        }

        /// <summary>
        /// 上级
        /// </summary>
        public string Parent
        {
            get { return valueDic.GetValue<string>("Parent"); }
            set { valueDic.SetValue("Parent", value); }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public string Root
        {
            get { return valueDic.GetValue<string>("Root"); }
            set { valueDic.SetValue("Root", value); }
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
        {
            get { return valueDic.GetValue<int>("Level"); }
            set { valueDic.SetValue("Level", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
        {
            get { return valueDic.GetValue<string>("Remark"); }
            set { valueDic.SetValue("Remark", value); }
        }

        #endregion
    }
}