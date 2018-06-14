using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 修改工作任务运行状态命令信息
    /// </summary>
    public class ModifyJobRunStateCmdDto
    {
        #region 属性

        /// <summary>
        /// 工作任务信息
        /// </summary>
        public IEnumerable<JobCmdDto> Jobs
        {
            get;set;
        }

        #endregion
    }
}
