using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Repository
{
    /// <summary>
    /// 任务异常日志存储
    /// </summary>
    public interface IErrorLogRepository : IRepository<ErrorLog>
    {
    }
}
