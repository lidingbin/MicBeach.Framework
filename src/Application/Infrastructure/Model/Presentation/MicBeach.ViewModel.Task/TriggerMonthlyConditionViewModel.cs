using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.ViewModel.Task
{
    /// <summary>
    /// 月份日期计划
    /// </summary>
    public class TriggerMonthlyConditionViewModel : TriggerConditionViewModel
    {
        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<MonthConditionDayViewModel> Days
        {
            get; set;
        }

        #endregion
    }
}
