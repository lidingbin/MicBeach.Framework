using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 任务工作文件
    /// </summary>
    [Serializable]
    public class JobFileEntity : CommandEntity<JobFileEntity>
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
        /// 工作
        /// </summary>
        public string Job
        {
            get { return valueDic.GetValue<string>("Job"); }
            set { valueDic.SetValue("Job", value); }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return valueDic.GetValue<string>("FileName"); }
            set { valueDic.SetValue("FileName", value); }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return valueDic.GetValue<string>("FilePath"); }
            set { valueDic.SetValue("FilePath", value); }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return valueDic.GetValue<DateTime>("CreateDate"); }
            set { valueDic.SetValue("CreateDate", value); }
        }

        #endregion
    }
}