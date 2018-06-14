using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Domain.Aggregation
{
    /// <summary>
    /// 数据变动委托
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="datas">数据值</param>
    public delegate void DataChange<T>(params T[] datas);

    /// <summary>
    /// 查询数据委托
    /// </summary>
    /// <param name="datas">数据值</param>
    /// <returns></returns>
    public delegate void QueryData<T>(ref IEnumerable<T> datas);
}
