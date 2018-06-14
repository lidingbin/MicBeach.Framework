using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 修改执行计划运行状态
    /// </summary>
    public class ModifyTriggerServerRunStateCmdDto
    {
        #region 属性

        /// <summary>
        /// 计划服务信息
        /// </summary>
        public List<TriggerServerCmdDto> TriggerServers
        {
            get; set;
        }

        #endregion
    }
}
