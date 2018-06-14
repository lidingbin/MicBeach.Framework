﻿using MicBeach.Develop.DataAccess;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataAccessInterface.Task
{
    /// <summary>
    /// 工作任务数据访问接口
    /// </summary>
    public interface IJobDataAccess : IDataAccess<JobEntity>
    {
    }

    /// <summary>
    /// 工作任务数据库接口
    /// </summary>
    public interface IJobDbAccess : IJobDataAccess
    {
    }
}
