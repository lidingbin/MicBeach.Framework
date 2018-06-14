using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 自定义表达式信息
    /// </summary>
    public class ExpressionTriggerCmdDto : TriggerCmdDto
    {
        #region	属性

        /// <summary>
        /// 表达式项
        /// </summary>
        public List<ExpressionItemCmdDto> ExpressionItems
        {
            get;set;
        }

        #endregion
    }
}
