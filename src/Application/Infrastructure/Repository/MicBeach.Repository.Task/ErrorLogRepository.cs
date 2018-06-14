using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Entity.Task;
using MicBeach.Domain.Task.Model;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Repository;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 任务异常日志存储
    /// </summary>
    public class ErrorLogRepository : DefaultRepository<ErrorLog, ErrorLogEntity, IErrorLogDataAccess>, IErrorLogRepository
    {
    }
}
