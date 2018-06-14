using MicBeach.Domain.Task.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Repository
{
    /// <summary>
    /// 表达式计划存储
    /// </summary>
    public interface IExpressionTriggerRepository
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="triggers">计划数据</param>
        void SaveExpressionTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="triggers">计划数据</param>
        void RemoveExpressionTrigger(IEnumerable<Trigger> triggers);

        /// <summary>
        /// 获取自定义计划数据
        /// </summary>
        /// <param name="triggers">计划数据</param>
        /// <returns></returns>
        void LoadExpressionTrigger(ref IEnumerable<Trigger> triggers);
    }
}
