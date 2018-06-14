using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 星期执行计划
    /// </summary>
    public class TriggerWeeklyConditionCmdDto : TriggerConditionCmdDto
    {
        #region	属性

        /// <summary>
        /// 日期
        /// </summary>
        public List<WeeklyConditionDayCmdDto> Days
        {
            get; set;
        }

        #endregion
    }
}
