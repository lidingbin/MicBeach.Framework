using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 计划附加条件
    /// </summary>
    public class TriggerConditionCmdDto
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string TriggerId
        {
            get;set;
        }

        /// <summary>
        /// 条件类型
        /// </summary>
        public TaskTriggerConditionType Type
        {
            get;set;
        }

        #endregion
    }
}