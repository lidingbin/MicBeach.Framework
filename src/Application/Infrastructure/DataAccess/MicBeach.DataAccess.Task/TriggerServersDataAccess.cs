using MicBeach.Develop.DataAccess;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataAccess.Task
{
    /// <summary>
    /// 服务节点执行计划数据访问
    /// </summary>
    public class TriggerServerDataAccess : RdbDataAccess<TriggerServerEntity>, ITriggerServerDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Trigger", "Server", "RunState", "LastFireDate", "NextFireDate" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Trigger", "Server", "RunState", "LastFireDate", "NextFireDate" };
        }

        #endregion
    }
}
