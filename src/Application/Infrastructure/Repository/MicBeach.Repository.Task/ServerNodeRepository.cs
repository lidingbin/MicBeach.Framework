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

namespace MicBeach.Repository.Task
{
    /// <summary>
    /// 服务节点存储
    /// </summary>
    public class ServerNodeRepository : DefaultRepository<ServerNode, ServerNodeEntity, IServerNodeDataAccess>, IServerNodeRepository
    {
        protected override void BindEvent()
        {
            IJobServerHostRepository jobServerHostRepository = this.Instance<IJobServerHostRepository>();
            RemoveEvent += jobServerHostRepository.RemoveJobServerHostByServer;//删除对应的工作承载信息

            //执行计划
            ITriggerServerRepository triggerServerRepository = this.Instance<ITriggerServerRepository>();
            RemoveEvent += triggerServerRepository.RemoveTriggerServerFromServer;
        }
    }
}
