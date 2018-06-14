using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Domain.Aggregation
{
    /// <summary>
    /// 对象生命周期状态
    /// </summary>
    public enum LifeStatus
    {
        New,
        Modify,
        Remove,
        Stored
    }
}
