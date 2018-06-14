using MicBeach.DataAccessInterface.Task;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Develop.DataAccess;

namespace MicBeach.DataAccess.Task
{
    /// <summary>
    /// 服务节点数据访问
    /// </summary>
    public class ServerNodeDataAccess : RdbDataAccess<ServerNodeEntity>, IServerNodeDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Id", "InstanceName", "Name", "State", "Host", "ThreadCount", "ThreadPriority", "Description" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Id", "InstanceName", "Name", "State", "Host", "ThreadCount", "ThreadPriority", "Description" };
        }

        #endregion
    }
}
