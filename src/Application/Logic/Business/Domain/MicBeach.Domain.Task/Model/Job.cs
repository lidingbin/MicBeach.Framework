using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.Application.Task;
using MicBeach.CTask;
using MicBeach.Util.Data;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 工作任务
    /// </summary>
    public class Job : AggregationRoot<Job>
    {
        IJobRepository jobRepository = null;

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected string _id;

        /// <summary>
        /// 分组
        /// </summary>
        protected LazyMember<JobGroup> _group;

        /// <summary>
        /// 名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 任务类型
        /// </summary>
        protected JobApplicationType _type;

        /// <summary>
        /// 执行类型
        /// </summary>
        protected JobRunType _runType;

        /// <summary>
        /// 状态
        /// </summary>
        protected JobState _state;

        /// <summary>
        /// 说明
        /// </summary>
        protected string _description;

        /// <summary>
        /// 更新时间
        /// </summary>
        protected DateTime _updateDate;

        /// <summary>
        /// 任务路径
        /// </summary>
        protected string _jobPath;

        /// <summary>
        /// 任务文件名称
        /// </summary>
        protected string _jobFileName;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化工作任务对象
        /// </summary>
        /// <param name="id">编号</param>
        internal Job(string id = "")
        {
            _id = id;
            jobRepository = this.Instance<IJobRepository>();
            _group = new LazyMember<JobGroup>(LoadGroup);
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string Id
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
        /// 分组
        /// </summary>
        public JobGroup Group
        {
            get
            {
                return _group.Value;
            }
            protected set
            {
                _group.SetValue(value, false);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        public JobApplicationType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        public JobRunType RunType
        {
            get
            {
                return _runType;
            }
            set
            {
                _runType = value;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public JobState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDate
        {
            get
            {
                return _updateDate;
            }
            protected set
            {
                _updateDate = value;
            }
        }

        /// <summary>
        /// 任务路径
        /// </summary>
        public string JobPath
        {
            get
            {
                return _jobPath;
            }
            set
            {
                _jobPath = value;
            }
        }

        /// <summary>
        /// 任务文件名称
        /// </summary>
        public string JobFileName
        {
            get
            {
                return _jobFileName;
            }
            set
            {
                _jobFileName = value;
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
            jobRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            jobRepository.Remove(this);
        }

        #endregion

        #region 设置任务分组

        /// <summary>
        /// 设置任务所属分组
        /// </summary>
        /// <param name="group">分组信息</param>
        /// <param name="init">是否用新的分组信息初始化任务的分组，若初始化将不会再自动去加载分组信息</param>
        public void SetGroup(JobGroup group, bool init = true)
        {
            if (group == null)
            {
                init = false;
            }
            _group.SetValue(group, init);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _id = GenerateJobId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载任务分组

        /// <summary>
        /// 加载任务分组
        /// </summary>
        /// <returns></returns>
        JobGroup LoadGroup()
        {
            if (!AllowLazyLoad(r => r.Group))
            {
                return _group.CurrentValue;
            }
            if (_group.CurrentValue == null || _group.CurrentValue.Code.IsNullOrEmpty())
            {
                return _group.CurrentValue;
            }
            return this.Instance<IJobGroupRepository>().Get(QueryFactory.Create<JobGroupQuery>(r => r.Code == _group.CurrentValue.Code));
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return _id.IsNullOrEmpty();
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个工作任务编号

        /// <summary>
        /// 生成一个工作任务编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateJobId()
        {
            return SerialNumber.GetSerialNumber(TaskApplicationUtil.GetIdGroupCode(TaskIdGroup.工作任务)).ToString();
        }

        #endregion

        #region 创建工作任务

        /// <summary>
        /// 创建一个工作任务对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static Job CreateJob(string id = "")
        {
            id = id.IsNullOrEmpty() ? GenerateJobId() : id;
            return new Job(id);
        }

        #endregion

        #endregion

        #endregion
    }
}