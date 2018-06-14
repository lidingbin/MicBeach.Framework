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
    /// 任务异常日志数据访问接口
    /// </summary>
    public interface IErrorLogDataAccess : IDataAccess<ErrorLogEntity>
    {
    }

    /// <summary>
    /// 任务异常日志数据库接口
    /// </summary>
    public interface IErrorLogDbAccess : IErrorLogDataAccess
    {
    }
}
