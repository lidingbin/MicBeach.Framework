using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
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
    /// 任务工作文件存储
    /// </summary>
    public interface IJobFileRepository : IRepository<JobFile>
    {
    }
}
