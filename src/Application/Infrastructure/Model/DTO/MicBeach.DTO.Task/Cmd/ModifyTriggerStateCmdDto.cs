using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 修改执行计划信息
    /// </summary>
    public class ModifyTriggerStateCmdDto
    {
        /// <summary>
        /// 执行计划信息
        /// </summary>
        public List<TriggerCmdDto> Triggers
        {
            get;set;
        }
    }
}
