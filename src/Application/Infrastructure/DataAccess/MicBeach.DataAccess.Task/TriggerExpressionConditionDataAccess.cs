﻿using MicBeach.DataAccessInterface.Task;
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
    /// 计划表达式附加条件数据访问
    /// </summary>
    public class TriggerExpressionConditionDataAccess : RdbDataAccess<TriggerExpressionConditionEntity>, ITriggerExpressionConditionDbAccess
    {
        #region 获取添加字段

        /// <summary>
        /// 获取添加字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetEditFields()
        {
            return new string[] { "TriggerId", "ConditionOption", "ValueType", "BeginValue", "EndValue", "ArrayValue" };
        }

        #endregion

        #region 获取查询字段

        /// <summary>
        /// 获取查询字段
        /// </summary>
        /// <returns></returns>
        protected override string[] GetQueryFields()
        {
            return new string[] { "TriggerId", "ConditionOption", "ValueType", "BeginValue", "EndValue", "ArrayValue" };
        }

        #endregion
    }
}
