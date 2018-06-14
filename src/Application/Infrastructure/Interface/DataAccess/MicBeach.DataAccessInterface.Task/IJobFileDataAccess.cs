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
    /// 任务工作文件数据访问接口
    /// </summary>
    public interface IJobFileDataAccess : IDataAccess<JobFileEntity>
    {
    }

    /// <summary>
    /// 任务工作文件数据库接口
    /// </summary>
    public interface IJobFileDbAccess : IJobFileDataAccess
    {
    }
}
