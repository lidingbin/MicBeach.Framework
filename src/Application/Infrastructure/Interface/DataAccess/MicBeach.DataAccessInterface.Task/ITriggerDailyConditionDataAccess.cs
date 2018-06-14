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
    /// 计划时间计划条件数据访问接口
    /// </summary>
    public interface ITriggerDailyConditionDataAccess : IDataAccess<TriggerDailyConditionEntity>
    {
    }

    /// <summary>
    /// 计划时间计划条件数据库接口
    /// </summary>
    public interface ITriggerDailyConditionDbAccess : ITriggerDailyConditionDataAccess
    {
    }
}
