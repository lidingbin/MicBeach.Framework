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
    /// 计划表达式附加条件数据访问接口
    /// </summary>
    public interface ITriggerExpressionConditionDataAccess : IDataAccess<TriggerExpressionConditionEntity>
    {
    }

    /// <summary>
    /// 计划表达式附加条件数据库接口
    /// </summary>
    public interface ITriggerExpressionConditionDbAccess : ITriggerExpressionConditionDataAccess
    {
    }
}
