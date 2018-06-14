using MicBeach.Develop.Domain.Repository;
using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Repository
{
    public interface ISimpleTriggerRepository : IRepository<SimpleTrigger>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="triggers">计划数据</param>
        void SaveSimpleTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="triggers">计划数据</param>
        void RemoveSimpleTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 获取简单计划
        /// </summary>
        /// <param name="triggers">计划数据</param>
        /// <returns></returns>
        void LoadSimpleTrigger(ref IEnumerable<Trigger> triggers);
    }
}
