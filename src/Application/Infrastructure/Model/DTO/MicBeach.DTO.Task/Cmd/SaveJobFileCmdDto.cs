using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.DTO.Task.Cmd
{
    /// <summary>
    /// 保存任务工作文件信息
    /// </summary>
    public class SaveJobFileCmdDto
    {
        /// <summary>
        /// 任务工作文件信息
        /// </summary>
        public JobFileCmdDto JobFile
        {
            get; set;
        }
    }
}
