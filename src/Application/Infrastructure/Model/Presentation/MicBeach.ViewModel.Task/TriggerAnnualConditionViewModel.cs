using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.ViewModel.Task
{
    /// <summary>
    /// 年度计划
    /// </summary>
    public class TriggerAnnualConditionViewModel : TriggerConditionViewModel
    {
        #region 属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<AnnualConditionDayViewModel> Days
        {
            get; set;
        }

        #endregion
    }
}
