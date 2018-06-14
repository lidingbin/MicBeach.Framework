using MicBeach.Application.Task;
using MicBeach.Util;
using MicBeach.Util.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Cms.Config
{
    /// <summary>
    /// 唯一标识符配置
    /// </summary>
    public static class IdentityKeyConfig
    {
        public static void Init()
        {
            List<string> groupCodes = new List<string>();

            #region Task

            Array taskValues = Enum.GetValues(TaskIdGroup.服务节点.GetType());
            foreach (TaskIdGroup group in taskValues)
            {
                groupCodes.Add(TaskApplicationUtil.GetIdGroupCode(group));
            }

            #endregion

            SerialNumber.RegisterGenerator(groupCodes, 1, 1);
        }
    }
}
