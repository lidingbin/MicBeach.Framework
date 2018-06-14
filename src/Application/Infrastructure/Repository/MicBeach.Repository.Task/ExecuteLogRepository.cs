using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using MicBeach.Entity.Task;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Domain.Task.Repository;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 任务执行日志存储
    /// </summary>
    public class ExecuteLogRepository : DefaultRepository<ExecuteLog, ExecuteLogEntity, IExecuteLogDataAccess>, IExecuteLogRepository
    {
    }
}
