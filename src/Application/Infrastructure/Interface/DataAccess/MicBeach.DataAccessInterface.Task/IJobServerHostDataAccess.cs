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
    /// 工作承载节点数据访问接口
    /// </summary>
    public interface IJobServerHostDataAccess : IDataAccess<JobServerHostEntity>
    {
    }

    /// <summary>
    /// 工作承载节点数据库接口
    /// </summary>
    public interface IJobServerHostDbAccess : IJobServerHostDataAccess
    {
    }
}
