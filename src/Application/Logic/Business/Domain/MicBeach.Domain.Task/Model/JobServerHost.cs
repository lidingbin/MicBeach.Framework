using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.Util.Data;
using MicBeach.CTask;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using System.Collections;
using System.Collections.Generic;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 工作承载节点
    /// </summary>
    public class JobServerHost : AggregationRoot<JobServerHost>
    {
        IJobServerHostRepository jobServerHostRepository = null;

        #region	字段

        /// <summary>
        /// 服务
        /// </summary>
        protected LazyMember<ServerNode> _server;

        /// <summary>
        /// 任务
        /// </summary>
        protected LazyMember<Job> _job;

        /// <summary>
        /// 运行状态
        /// </summary>
        protected JobServerState _runState;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化工作承载节点对象
        /// </summary>
        /// <param name="server">编号</param>
        internal JobServerHost()
        {
            _server = new LazyMember<ServerNode>(LoadServer);
            _job = new LazyMember<Job>(LoadJob);
            jobServerHostRepository = this.Instance<IJobServerHostRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 服务
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
        /// 任务
        /// </summary>
        public Job Job
        {
            get
            {
                return _job.Value;
            }
            protected set
            {
                _job.SetValue(value, false); ;
            }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerState RunState
        {
            get
            {
                return _runState;
            }
            set
            {
                _runState = value;
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
            jobServerHostRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            jobServerHostRepository.Remove(this);
        }

        #endregion

        #region 设置工作任务

        /// <summary>
        /// 设置工作任务
        /// </summary>
        /// <param name="job">工作信息</param>
        /// <param name="init">是否初始化，初始化后将不会自动加载数据</param>
        public void SetJob(Job job, bool init = true)
        {
            _job.SetValue(job, init);
        }

        #endregion

        #region 设置服务信息

        /// <summary>
        /// 设置服务信息
        /// </summary>
        /// <param name="server">服务信息</param>
        /// <param name="init">是否初始化，初始化后将不会自动加载数据</param>
        public void SetServer(ServerNode server, bool init = true)
        {
            _server.SetValue(server, init);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 加载服务

        /// <summary>
        /// 加载服务
        /// </summary>
        /// <returns></returns>
        ServerNode LoadServer()
        {
            if (!AllowLazyLoad(r => r.Server))
            {
                return _server.CurrentValue;
            }
            if (_server.CurrentValue == null || _server.CurrentValue.Id.IsNullOrEmpty())
            {
                return _server.CurrentValue;
            }
            return this.Instance<IServerNodeRepository>().Get(QueryFactory.Create<ServerNodeQuery>(r => r.Id == _server.CurrentValue.Id));
        }

        #endregion

        #region 加载工作信息

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
            if (_job.CurrentValue == null || _job.CurrentValue.Id.IsNullOrEmpty())
            {
                return _job.CurrentValue;
            }
            return this.Instance<IJobRepository>().Get(QueryFactory.Create<JobQuery>(r => r.Id == _job.CurrentValue.Id));
        }

        #endregion

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return (_job.Value?.Id.IsNullOrEmpty() ?? true) || (_server.Value?.Id.IsNullOrEmpty() ?? true);
        }

        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 创建任务&服务承载信息
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="serverId">服务编号</param>
        /// <returns></returns>
        public static JobServerHost CreateJobServerHost(string jobId, string serverId)
        {
            var jobServerHost = new JobServerHost();
            jobServerHost.SetJob(Job.CreateJob(jobId));
            jobServerHost.SetServer(ServerNode.CreateServerNode(serverId));
            return jobServerHost;
        }

        #endregion

        #endregion
    }

    public class JobServerHostCompare : IEqualityComparer<JobServerHost>
    {
        public bool Equals(JobServerHost x, JobServerHost y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Server?.Id == y.Server?.Id && x.Job?.Id == y.Job?.Id;
        }

        public int GetHashCode(JobServerHost obj)
        {
            return 0;
        }
    }
}