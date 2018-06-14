using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.CTask;
using System.Collections.Generic;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 服务节点执行计划
    /// </summary>
    public class TriggerServer : AggregationRoot<TriggerServer>
    {
        ITriggerServerRepository triggerServerRepository = null;

        #region	字段

        /// <summary>
        /// 触发器
        /// </summary>
        protected Trigger _trigger;

        /// <summary>
        /// 服务
        /// </summary>
        protected ServerNode _server;

        /// <summary>
        /// 运行状态
        /// </summary>
        protected TaskTriggerServerRunState _runState;

        /// <summary>
        /// 上次执行时间
        /// </summary>
        protected DateTime _lastFireDate;

        /// <summary>
        /// 下次执行时间
        /// </summary>
        protected DateTime _nextFireDate;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化服务节点执行计划对象
        /// </summary>
        /// <param name="trigger">编号</param>
        internal TriggerServer()
        {
            triggerServerRepository = this.Instance<ITriggerServerRepository>();
        }

        #endregion

        #region	属性

        /// <summary>
        /// 触发器
        /// </summary>
        public Trigger Trigger
        {
            get
            {
                return _trigger;
            }
            protected set
            {
                _trigger = value;
            }
        }

        /// <summary>
        /// 服务
        /// </summary>
        public ServerNode Server
        {
            get
            {
                return _server;
            }
            protected set
            {
                _server = value;
            }
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public TaskTriggerServerRunState RunState
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

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime LastFireDate
        {
            get
            {
                return _lastFireDate;
            }
            protected set
            {
                _lastFireDate = value;
            }
        }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime NextFireDate
        {
            get
            {
                return _nextFireDate;
            }
            protected set
            {
                _nextFireDate = value;
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
            if (SaveByAdd)
            {
                LastFireDate = DateTime.Now;
                NextFireDate = DateTime.Now;
            }
            triggerServerRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            triggerServerRepository.Remove(this);
        }

        #endregion

        #endregion

        #region 内部方法

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return _trigger == null || _server == null;
        }

        #endregion

        #endregion

        #region 静态方法

        #region 生成一个服务节点执行计划编号

        /// <summary>
        /// 生成一个服务节点执行计划编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateTriggerServerId()
        {
            return SerialNumber.GetSerialNumber("").ToString();
        }

        #endregion

        #region 创建服务节点执行计划

        /// <summary>
        /// 创建一个服务节点执行计划对象
        /// </summary>
        /// <param name="trigger">编号</param>
        /// <returns></returns>
        public static TriggerServer CreateTriggerServer(string triggerId, string serverId, TaskTriggerServerRunState runState = TaskTriggerServerRunState.运行)
        {
            return CreateTriggerServer(Trigger.CreateTrigger(triggerId), ServerNode.CreateServerNode(serverId), runState);
        }

        /// <summary>
        /// 创建一个计划服务对象
        /// </summary>
        /// <param name="trigger">执行计划</param>
        /// <param name="server">服务信息</param>
        /// <param name="runState">运行状态</param>
        /// <returns></returns>
        public static TriggerServer CreateTriggerServer(Trigger trigger, ServerNode server, TaskTriggerServerRunState runState = TaskTriggerServerRunState.运行)
        {
            return new TriggerServer()
            {
                _server = server,
                _trigger = trigger,
                _runState = runState
            };
        }

        #endregion

        #endregion

        #endregion
    }

    public class TriggerServerCompare : IEqualityComparer<TriggerServer>
    {
        public bool Equals(TriggerServer x, TriggerServer y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Server?.Id == y.Server?.Id && x.Trigger?.Id == y.Trigger?.Id;
        }

        public int GetHashCode(TriggerServer obj)
        {
            return 0;
        }
    }
}