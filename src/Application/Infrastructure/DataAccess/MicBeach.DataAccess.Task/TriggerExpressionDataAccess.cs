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
    /// 自定义表达式计划数据访问
    /// </summary>
    public class TriggerExpressionDataAccess : RdbDataAccess<TriggerExpressionEntity>, ITriggerExpressionDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "TriggerId", "Option", "ValueType", "BeginValue", "EndValue", "ArrayValue" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "TriggerId", "Option", "ValueType", "BeginValue", "EndValue", "ArrayValue" };
        }

        #endregion
    }
}
