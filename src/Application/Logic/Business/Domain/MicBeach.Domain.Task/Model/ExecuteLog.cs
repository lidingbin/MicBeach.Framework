using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.Util.Data;
using MicBeach.CTask;
using MicBeach.Domain.Task.Service;
using MicBeach.Application.Task;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 任务执行日志
    /// </summary>
    public class ExecuteLog : AggregationRoot<ExecuteLog>
    {
        IExecuteLogRepository executeLogRepository = null;

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected long _id;

        /// <summary>
        /// 工作任务
        /// </summary>
        protected LazyMember<Job> _job;

        /// <summary>
        /// 执行计划
        /// </summary>
        protected LazyMember<Trigger> _trigger;

        /// <summary>
        /// 执行服务
        /// </summary>
        protected LazyMember<ServerNode> _server;

        /// <summary>
        /// 开始时间
        /// </summary>
        protected DateTime _beginTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        protected DateTime _endTime;

        /// <summary>
        /// 记录时间
        /// </summary>
        protected DateTime _recordTime;

        /// <summary>
        /// 执行状态
        /// </summary>
        protected ExecuteLogState _state;

        /// <summary>
        /// 消息
        /// </summary>
        protected string _message;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化任务执行日志对象
        /// </summary>
        /// <param name="id">编号</param>
        internal ExecuteLog(long id = 0)
        {
            _id = id;
            _job = new LazyMember<Model.Job>(LoadJob);
            _trigger = new LazyMember<Model.Trigger>(LoadTrigger);
            _server = new LazyMember<ServerNode>(LoadServer);
            executeLogRepository = this.Instance<IExecuteLogRepository>();
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
        /// 工作任务
        /// </summary>
        public Job Job
        {
            get
            {
                return _job.Value;
            }
            protected set
            {
                _job.SetValue(value, false);
            }
        }

        /// <summary>
        /// 执行计划
        /// </summary>
        public Trigger Trigger
        {
            get
            {
                return _trigger.Value;
            }
            protected set
            {
                _trigger.SetValue(value, false);
            }
        }

        /// <summary>
        /// 执行服务
        /// </summary>
        public ServerNode Server
        {
            get
            {
                return _server.Value;
            }
            protected set
            {
                _server.SetValue(value, false);
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get
            {
                return _beginTime;
            }
            protected set
            {
                _beginTime = value;
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            protected set
            {
                _endTime = value;
            }
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime
        {
            get
            {
                return _recordTime;
            }
            protected set
            {
                _recordTime = value;
            }
        }

        /// <summary>
        /// 执行状态
        /// </summary>
        public ExecuteLogState State
        {
            get
            {
                return _state;
            }
            protected set
            {
                _state = value;
            }
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
            protected set
            {
                _message = value;
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
            executeLogRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            executeLogRepository.Remove(this);
        }

        #endregion

        #region 设置服务信息

        /// <summary>
        /// 设置服务节点
        /// </summary>
        /// <param name="server">服务信息</param>
        /// <param name="init">是否初始化（初始化后将不会再自动加载数据）</param>
        public void SetServer(ServerNode server, bool init = true)
        {
            _server.SetValue(server, init);
        }

        #endregion

        #region 设置工作任务

        /// <summary>
        /// 设置工作任务
        /// </summary>
        /// <param name="job">工作信息</param>
        /// <param name="init">是否初始化（初始化后将不会再自动加载数据）</param>
        public void SetJob(Job job, bool init = true)
        {
            _job.SetValue(job, init);
        }

        #endregion

        #region 设置执行计划

        /// <summary>
        /// 设置执行计划
        /// </summary>
        /// <param name="trigger">执行计划信息</param>
        /// <param name="init">是否初始化（初始化后将不会再自动加载数据）</param>
        public void SetTrigger(Trigger trigger, bool init = true)
        {
            _trigger.SetValue(trigger, init);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _id = GenerateExecuteLogId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载工作

        /// <summary>
        /// 加载工作信息
        /// </summary>
        /// <returns></returns>
        Job LoadJob()
        {
            if (!AllowLazyLoad(r => r.Job))
            {
                return _job.CurrentValue;
            }
            if (_job.CurrentValue == null)
            {
                return _job.CurrentValue;
            }
            return JobService.GetJob(_job.CurrentValue.Id);
        }

        #endregion

        #region 加载执行计划

        Trigger LoadTrigger()
        {
            if (!AllowLazyLoad(r => r.Trigger))
            {
                return _trigger.CurrentValue;
            }
            if (_trigger.CurrentValue == null)
            {
                return _trigger.CurrentValue;
            }
            return TriggerService.GetTrigger(_trigger.CurrentValue.Id);
        }

        #endregion

        #region 加载服务

        ServerNode LoadServer()
        {
            if (!AllowLazyLoad(r => r.Server))
            {
                return _server.CurrentValue;
            }
            if (_server.CurrentValue == null)
            {
                return _server.CurrentValue;
            }
            return ServerNodeService.GetServerNode(_server.CurrentValue.Id);
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return _id <= 0;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个任务执行日志编号

        /// <summary>
        /// 生成一个任务执行日志编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateExecuteLogId()
        {
            return SerialNumber.GetSerialNumber(TaskApplicationUtil.GetIdGroupCode(TaskIdGroup.执行日志));
        }

        #endregion

        #region 创建任务执行日志

        /// <summary>
        /// 创建一个任务执行日志对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static ExecuteLog CreateExecuteLog(long id = 0)
        {
            id = id <= 0 ? GenerateExecuteLogId() : id;
            return new ExecuteLog(id);
        }

        #endregion

        #endregion

        #endregion
    }
}