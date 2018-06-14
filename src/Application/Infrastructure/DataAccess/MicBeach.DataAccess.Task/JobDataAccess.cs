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
    /// 工作任务数据访问
    /// </summary>
    public class JobDataAccess : RdbDataAccess<JobEntity>, IJobDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Id", "Group", "Name", "Type", "RunType", "State", "Description", "UpdateDate", "JobPath", "JobFileName" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Id", "Group", "Name", "Type", "RunType", "State", "Description", "UpdateDate", "JobPath", "JobFileName" };
        }

        #endregion
    }
}
