using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 修改服务承载运行状态
    /// </summary>
    public class ModifyJobServerHostRunStateCmdDto
    {
        /// <summary>
        /// 要修改服务承载信息
        /// </summary>
        public List<JobServerHostCmdDto> JobServerHosts
        {
            get; set;
        }
    }
}
