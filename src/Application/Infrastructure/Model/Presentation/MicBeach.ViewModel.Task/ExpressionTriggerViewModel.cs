using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.ViewModel.Task
{
    /// <summary>
    /// 自定义执行计划
    /// </summary>
    public class ExpressionTriggerViewModel:TriggerViewModel
    {
        #region	属性

        /// <summary>
        /// 表达式
        /// </summary>
        public List<ExpressionItemViewModel> ExpressionItems
        {
            get;
            set;
        }

        #endregion
    }
}
