using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.Application.Task;
using MicBeach.Util.Data;
using MicBeach.Domain.Task.Service;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 任务异常日志
    /// </summary>
    public class ErrorLog : AggregationRoot<ErrorLog>
    {
        IErrorLogRepository errorLogRepository = null;

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
        /// 错误消息
        /// </summary>
        protected string _message;

        /// <summary>
        /// 错误描述
        /// </summary>
        protected string _description;

        /// <summary>
        /// 错误类型
        /// </summary>
        protected int _type;

        /// <summary>
        /// 时间
        /// </summary>
        protected DateTime _date;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化任务异常日志对象
        /// </summary>
        /// <param name="id">编号</param>
        internal ErrorLog(long id = 0)
        {
            _id = id;
            _job = new LazyMember<Model.Job>(LoadJob);
            _trigger = new LazyMember<Model.Trigger>(LoadTrigger);
            _server = new LazyMember<ServerNode>(LoadServer);
            errorLogRepository = this.Instance<IErrorLogRepository>();
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
        /// 服务节点
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
        /// 错误消息
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

        /// <summary>
        /// 错误描述
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            protected set
            {
                _description = value;
            }
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        public int Type
        {
            get
            {
                return _type;
            }
            protected set
            {
                _type = value;
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Date
        {
            get
            {
                return _date;
            }
            protected set
            {
                _date = value;
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
            errorLogRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            errorLogRepository.Remove(this);
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
            _id = GenerateErrorLogId();
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

        #region 生成一个任务异常日志编号

        /// <summary>
        /// 生成一个任务异常日志编号
        /// </summary>
        /// <returns></returns>
        public static long GenerateErrorLogId()
        {
            return SerialNumber.GetSerialNumber(TaskApplicationUtil.GetIdGroupCode(TaskIdGroup.错误日志));
        }

        #endregion

        #region 创建任务异常日志

        /// <summary>
        /// 创建一个任务异常日志对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static ErrorLog CreateErrorLog(long id = 0)
        {
            id = id <= 0 ? GenerateErrorLogId() : id;
            return new ErrorLog(id);
        }

        #endregion

        #endregion

        #endregion
    }
}