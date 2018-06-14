using MicBeach.Util.Paging;
using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.ViewModel.Task.Filter
{
    /// <summary>
    /// 服务节点筛选
    /// </summary>
    public class ServerNodeFilterViewModel : PagingFilter
    {
        /// <summary>
        /// 服务节点
        /// </summary>
        public IEnumerable<string> Servers
        {
            get; set;
        }

        /// <summary>
        /// 服务关键字
        /// </summary>
        public string ServerKey
        {
            get; set;
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        public IEnumerable<string> Jobs
        {
            get; set;
        }

        /// <summary>
        /// 工作关键字
        /// </summary>
        public string JobKey
        {
            get; set;
        }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobServerState? RunState
        {
            get; set;
        }

        /// <summary>
        /// 不包含已绑定指定任务的
        /// </summary>
        public IEnumerable<string> ExcludeBindJobIds
        {
            get; set;
        }
    }
}
