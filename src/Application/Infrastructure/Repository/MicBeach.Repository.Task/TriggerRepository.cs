using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Domain.Task.Model;
using MicBeach.Develop.UnitOfWork;
using MicBeach.Develop.Domain.Repository;
using MicBeach.Entity.Task;
using MicBeach.Util.Extension;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.CQuery;
using MicBeach.Util.Paging;
using MicBeach.Domain.Task.Repository;
using MicBeach.Query.Task;

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 任务执行计划存储
    /// </summary>
    public class TriggerRepository : DefaultRepository<Trigger, TriggerEntity, ITriggerDataAccess>, ITriggerRepository
    {
        public TriggerRepository()
        {

        }

        #region 删除工作任务时删除相应的计划

        /// <summary>
        /// 删除工作任务时删除相应的计划
        /// </summary>
        /// <param name="jobs">工作任务</param>
        public void RemoveTriggerFromJob(IEnumerable<Job> jobs)
        {
            if (jobs.IsNullOrEmpty())
            {
                return;
            }
            List<string> jobIds = jobs.Select(c => c.Id).Distinct().ToList();
            IQuery removeQuery = QueryFactory.Create<TriggerQuery>(c => jobIds.Contains(c.Job));
            Remove(removeQuery);
        }

        #endregion

        protected override void BindEvent()
        {
            ISimpleTriggerRepository simpleTriggerRepository = this.Instance<ISimpleTriggerRepository>();
            IExpressionTriggerRepository expressionTriggerRepository = this.Instance<IExpressionTriggerRepository>();
            ITriggerConditionRepository triggerConditionRepository = this.Instance<ITriggerConditionRepository>();
            ITriggerServerRepository triggerServerRepository = this.Instance<ITriggerServerRepository>();

            //简单执行计划
            SaveEvent += simpleTriggerRepository.SaveSimpleTrigger;
            RemoveEvent += simpleTriggerRepository.RemoveSimpleTrigger;
            QueryEvent += simpleTriggerRepository.LoadSimpleTrigger;

            //自定义执行计划
            SaveEvent += expressionTriggerRepository.SaveExpressionTrigger;
            RemoveEvent += expressionTriggerRepository.RemoveExpressionTrigger;
            QueryEvent += expressionTriggerRepository.LoadExpressionTrigger;

            //附加条件
            SaveEvent += triggerConditionRepository.SaveTriggerConditionFromTrigger;
            RemoveEvent += triggerConditionRepository.RemoveTriggerConditionFromTrigger;

            //计划关联的服务
            RemoveEvent += triggerServerRepository.RemoveTriggerServerFromTrigger;
        }
    }
}
