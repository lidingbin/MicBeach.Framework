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
    /// 固定日期条件存储接口
    /// </summary>
    public interface ITriggerFullDateConditionRepository: IRepository<TriggerFullDateCondition>, IBaseTriggerConditionRepository
    {
    }
}
