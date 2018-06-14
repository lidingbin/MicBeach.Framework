using MicBeach.Develop.Domain.Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.CTask;
using MicBeach.Util;
using MicBeach.Application.Task;
using MicBeach.Util.Extension;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 执行计划附加条件
    /// </summary>
    public class TriggerCondition : AggregationRoot<TriggerCondition>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        protected string _triggerId;

        /// <summary>
        /// 条件类型
        /// </summary>
        protected TaskTriggerConditionType _type;

        #endregion

        #region 构造方法

        public TriggerCondition(string id = "")
        {
            _triggerId = id;
        }

        #endregion

        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string TriggerId
        {
            get
            {
                return _triggerId;
            }
            set
            {
                _triggerId = value;
            }
        }

        /// <summary>
        /// 条件类型
        /// </summary>
        public TaskTriggerConditionType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        #endregion

        #region 方法

        #region 功能方法

        /// <summary>
        /// 保存
        /// </summary>
        public override void Save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除
        /// </summary>
        public override void Remove()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 内部方法

        #region 验证对象标识信息是否未设置

        /// <summary>
        /// 判断对象标识信息是否未设置
        /// </summary>
        /// <returns></returns>
        protected override bool PrimaryValueIsNone()
        {
            return _triggerId.IsNullOrEmpty();
        }

        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 创建新的计划条件
        /// </summary>
        /// <param name="conditionType">条件类型</param>
        /// <param name="triggerId">计划编号</param>
        /// <returns></returns>
        public static TriggerCondition CreateTriggerCondition(TaskTriggerConditionType conditionType, string triggerId)
        {
            TriggerCondition condition = null;
            switch (conditionType)
            {
                case TaskTriggerConditionType.固定日期:
                    condition = new TriggerFullDateCondition(triggerId);
                    break;
                case TaskTriggerConditionType.星期配置:
                    condition = new TriggerWeeklyCondition(triggerId);
                    break;
                case TaskTriggerConditionType.每天时间段:
                    condition = new TriggerDailyCondition(triggerId);
                    break;
                case TaskTriggerConditionType.每年日期:
                    condition = new TriggerAnnualCondition(triggerId);
                    break;
                case TaskTriggerConditionType.每月日期:
                    condition = new TriggerMonthlyCondition(triggerId);
                    break;
                case TaskTriggerConditionType.自定义:
                    condition = new TriggerExpressionCondition(triggerId);
                    break;
            }
            return condition;
        }

        #endregion

        #endregion
    }
}
