using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.Util.Code;
using MicBeach.Application.Task;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 任务工作文件
    /// </summary>
    public class JobFile : AggregationRoot<JobFile>
    {
        IJobFileRepository jobFileRepository = null;

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected long _id;

        /// <summary>
        /// 工作
        /// </summary>
        protected string _job;

        /// <summary>
        /// 文件名称
        /// </summary>
        protected string _fileName;

        /// <summary>
        /// 文件路径
        /// </summary>
        protected string _filePath;

        /// <summary>
        /// 添加时间
        /// </summary>
        protected DateTime _createDate;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化任务工作文件对象
        /// </summary>
        /// <param name="id">编号</param>
        internal JobFile(long id = 0)
        {
            _id = id;
            jobFileRepository = this.Instance<IJobFileRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public long Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        /// <summary>
        /// 工作
        /// </summary>
        public string Job
        {
            get
            {
                return _job;
            }
            protected set
            {
                _job = value;
            }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            protected set
            {
                _fileName = value;
            }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            protected set
            {
                _filePath = value;
            }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
            protected set
            {
                _createDate = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        public override void Save()
        {
            jobFileRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            jobFileRepository.Remove(this);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _id = GenerateJobFileId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 判断标识对象值是否为空

        /// <summary>
        /// 判断标识对象值是否为空
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return _id <= 0;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个任务工作文件编号

        /// <summary>
        /// 生成一个任务工作文件编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateJobFileId()
        {
            return SerialNumber.GetSerialNumber(TaskApplicationUtil.GetIdGroupCode(TaskIdGroup.工作任务));
        }

        #endregion

        #region 创建任务工作文件

        /// <summary>
        /// 创建一个任务工作文件对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static JobFile CreateJobFile(long id = 0)
        {
            id = id <= 0 ? GenerateJobFileId() : id;
            return new JobFile(id);
        }

        #endregion

        #endregion

        #endregion
    }
}