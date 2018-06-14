using System;
using MicBeach.Develop.Domain.Aggregation;
using MicBeach.Util;
using MicBeach.Domain.Task.Repository;
using MicBeach.Util.Extension;
using MicBeach.CTask;
using MicBeach.Application.Task;
using MicBeach.Util.Code;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 服务节点
    /// </summary>
    public class ServerNode : AggregationRoot<ServerNode>
    {
        IServerNodeRepository serverNodeRepository = null;

        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected string _id;

        /// <summary>
        /// 实例名称
        /// </summary>
        protected string _instanceName;

        /// <summary>
        /// 节点名称
        /// </summary>
        protected string _name;

        /// <summary>
        /// 状态
        /// </summary>
        protected ServerState _state;

        /// <summary>
        /// 地址
        /// </summary>
        protected string _host;

        /// <summary>
        /// 线程数量
        /// </summary>
        protected int _threadCount;

        /// <summary>
        /// 线程优先级
        /// </summary>
        protected string _threadPriority;

        /// <summary>
        /// 说明
        /// </summary>
        protected string _description;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化服务节点对象
        /// </summary>
        /// <param name="id">编号</param>
        internal ServerNode(string id = "")
        {
            _id = id;
            serverNodeRepository = this.Instance<IServerNodeRepository>();
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
        /// 实例名称
        /// </summary>
        public string InstanceName
        {
            get
            {
                return _instanceName;
            }
            set
            {
                _instanceName = value;
            }
        }

        /// <summary>
        /// 节点名称
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
        /// 状态
        /// </summary>
        public ServerState State
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
        /// 地址
        /// </summary>
        public string Host
        {
            get
            {
                return _host;
            }
            set
            {
                _host = value;
            }
        }

        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount
        {
            get
            {
                return _threadCount;
            }
            set
            {
                _threadCount = value;
            }
        }

        /// <summary>
        /// 线程优先级
        /// </summary>
        public string ThreadPriority
        {
            get
            {
                return _threadPriority;
            }
            set
            {
                _threadPriority = value;
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

        #endregion

        #region 方法

        #region 功能方法

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        public override void Save()
        {
            serverNodeRepository.Save(this);
        }

        #endregion

        #region	移除

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            serverNodeRepository.Remove(this);
        }

        #endregion

        #region 初始化标识信息

        /// <summary>
        /// 初始化标识信息
        /// </summary>
        public override void InitPrimaryValue()
        {
            base.InitPrimaryValue();
            _id = GenerateServerNodeId();
        }

        #endregion

        #endregion

        #region 内部方法

        #region 保存数据验证

        /// <summary>
        /// 保存数据验证
        /// </summary>
        /// <returns></returns>
        protected override bool SaveValidation()
        {
            _instanceName = _instanceName ?? string.Empty;
            _description = _description ?? string.Empty;
            return base.SaveValidation();
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

        #region 生成一个服务节点编号

        /// <summary>
        /// 生成一个服务节点编号
        /// </summary>
        /// <returns></returns>
        public static string GenerateServerNodeId()
        {
            return SerialNumber.GetSerialNumber(TaskApplicationUtil.GetIdGroupCode(TaskIdGroup.服务节点)).ToString();
        }

        #endregion

        #region 创建服务节点

        /// <summary>
        /// 创建一个服务节点对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public static ServerNode CreateServerNode(string id = "")
        {
            id = id.IsNullOrEmpty() ? GenerateServerNodeId() : id;
            return new ServerNode(id);
        }

        #endregion

        #endregion

        #endregion
    }
}