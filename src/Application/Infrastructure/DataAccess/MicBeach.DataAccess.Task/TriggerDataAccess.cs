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
    /// 任务执行计划数据访问
    /// </summary>
    public class TriggerDataAccess : RdbDataAccess<TriggerEntity>, ITriggerDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Id", "Name", "Description", "Job", "ApplyTo", "PrevFireTime", "NextFireTime", "Priority", "State", "Type", "ConditionType", "StartTime", "EndTime", "MisFireType", "FireTotalCount" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Id", "Name", "Description", "Job", "ApplyTo", "PrevFireTime", "NextFireTime", "Priority", "State", "Type", "ConditionType", "StartTime", "EndTime", "MisFireType", "FireTotalCount" };
        }

        #endregion
    }
}
