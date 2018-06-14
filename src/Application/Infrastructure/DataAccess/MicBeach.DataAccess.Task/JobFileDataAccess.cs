using MicBeach.DataAccessInterface.Task;
using MicBeach.Develop.DataAccess;
using MicBeach.Entity.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DataAccess.Task
{
    /// <summary>
    /// 任务工作文件数据访问
    /// </summary>
    public class JobFileDataAccess : RdbDataAccess<JobFileEntity>, IJobFileDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "Id", "Job", "FileName", "FilePath", "CreateDate" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "Id", "Job", "FileName", "FilePath", "CreateDate" };
        }

        #endregion
    }
}
