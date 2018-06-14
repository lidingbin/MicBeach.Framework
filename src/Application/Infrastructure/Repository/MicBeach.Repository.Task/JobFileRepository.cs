using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Domain.Task.Model;
using MicBeach.Domain.Task.Repository;
using MicBeach.Entity.Task;
using MicBeach.Util.Extension;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.CQuery;
using MicBeach.Query.Task;
using MicBeach.Develop.Domain.Repository;


namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 任务工作文件存储
    /// </summary>
    public class JobFileRepository : DefaultRepository<JobFile, JobFileEntity, IJobFileDataAccess>, IJobFileRepository
    {
    }
}
