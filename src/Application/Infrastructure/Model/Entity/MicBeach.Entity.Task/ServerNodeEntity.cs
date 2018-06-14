using System;
using MicBeach.Develop.Command;
using MicBeach.Util.Extension;

namespace MicBeach.Entity.Task
{
    /// <summary>
    /// 服务节点
    /// </summary>
    [Serializable]
    public class ServerNodeEntity : CommandEntity<ServerNodeEntity>
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
        /// 实例名称
        /// </summary>
        public string InstanceName
        {
            get { return valueDic.GetValue<string>("InstanceName"); }
            set { valueDic.SetValue("InstanceName", value); }
        }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name
        {
            get { return valueDic.GetValue<string>("Name"); }
            set { valueDic.SetValue("Name", value); }
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
        /// 地址
        /// </summary>
        public string Host
        {
            get { return valueDic.GetValue<string>("Host"); }
            set { valueDic.SetValue("Host", value); }
        }

        /// <summary>
        /// 线程数量
        /// </summary>
        public int ThreadCount
        {
            get { return valueDic.GetValue<int>("ThreadCount"); }
            set { valueDic.SetValue("ThreadCount", value); }
        }

        /// <summary>
        /// 线程优先级
        /// </summary>
        public string ThreadPriority
        {
            get { return valueDic.GetValue<string>("ThreadPriority"); }
            set { valueDic.SetValue("ThreadPriority", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description
        {
            get { return valueDic.GetValue<string>("Description"); }
            set { valueDic.SetValue("Description", value); }
        }

        #endregion
    }
}