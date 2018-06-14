using MicBeach.DataAccessInterface.Task;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Develop.DataAccess;

namespace MicBeach.DataAccess.Task
{
    /// <summary>
    /// 任务执行日志数据访问
    /// </summary>
    public class ExecuteLogDataAccess : RdbDataAccess<ExecuteLogEntity>, IExecuteLogDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Id", "Job", "Trigger", "Server", "BeginTime", "EndTime", "RecordTime", "State", "Message" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Id", "Job", "Trigger", "Server", "BeginTime", "EndTime", "RecordTime", "State", "Message" };
        }

        #endregion
    }
}
