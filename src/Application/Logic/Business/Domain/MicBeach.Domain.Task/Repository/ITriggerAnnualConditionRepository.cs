using MicBeach.Develop.Domain.Aggregation;
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
    /// 年度计划存储接口
    /// </summary>
    public interface ITriggerAnnualConditionRepository: IRepository<TriggerAnnualCondition>,IBaseTriggerConditionRepository
    {
    }
}
