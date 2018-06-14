using MicBeach.Develop.DataAccess;
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
    /// 工作分组数据访问
    /// </summary>
    public class JobGroupDataAccess : RdbDataAccess<JobGroupEntity>, IJobGroupDataAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Code", "Name", "Sort", "Parent", "Root", "Level", "Remark" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Code", "Name", "Sort", "Parent", "Root", "Level", "Remark" };
        }

        #endregion
    }
}
