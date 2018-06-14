using MicBeach.Develop.DataAccess;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataAccessInterface.Task
{
    /// <summary>
    /// 服务节点数据访问接口
    /// </summary>
    public interface IServerNodeDataAccess : IDataAccess<ServerNodeEntity>
    {
    }

    /// <summary>
    /// 数据库访问
    /// </summary>
    public interface IServerNodeDbAccess : IServerNodeDataAccess
    { }
}
