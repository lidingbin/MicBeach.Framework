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
    /// 每月日期存储
    /// </summary>
    public interface ITriggerMonthlyConditionRepository: IRepository<TriggerMonthlyCondition>, IBaseTriggerConditionRepository
    {
    }
}
