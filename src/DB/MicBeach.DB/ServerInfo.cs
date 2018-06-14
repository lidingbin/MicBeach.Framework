using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DB
{
    /// <summary>
    /// 命令执行服务器信息
    /// </summary>
    public class ServerInfo
    {
        #region 字段

        string _key = string.Empty;
        string _connectionString = string.Empty;
        ServerType _serverType;

        #endregion

        #region 属性

        /// <summary>
        /// 服务器标识
        /// </summary>
        public string Key
        {
            get
            {
                return _key;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                InitKey();
            }
        }

        /// <summary>
        /// 服务器类型
        /// </summary>
        public ServerType ServerType
        {
            get
            {
                return _serverType;
            }
            set
            {
                _serverType = value;
                InitKey();
            }
        }

        #endregion

        #region 方法

        void InitKey()
        {
            _key = string.Format("{0}_{1}", (int)_serverType, _connectionString);
        }

        public override bool Equals(object otherServer)
        {
            if (otherServer == null)
            {
                return false;
            }
            ServerInfo otherServerInfo = otherServer as ServerInfo;
            if (otherServerInfo == null)
            {
                return false;
            }
            return _key == otherServerInfo.Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        #endregion
    }
}
