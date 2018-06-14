using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 时间段调价
    /// </summary>
    public class TriggerDailyCondition : TriggerCondition
    {
        #region	字段

        /// <summary>
        /// 开始时间
        /// </summary>
        protected string _beginTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        protected string _endTime;

        /// <summary>
        /// 启用设定值范围以外
        /// </summary>
        protected bool _inversion;

        #endregion

        #region	属性

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime
        {
            get
            {
                return _beginTime;
            }
            protected set
            {
                _beginTime = value;
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime
        {
            get
            {
                return _endTime;
            }
            protected set
            {
                _endTime = value;
            }
        }

        /// <summary>
        /// 启用设定值范围以外
        /// </summary>
        public bool Inversion
        {
            get
            {
                return _inversion;
            }
            protected set
            {
                _inversion = value;
            }
        }

        #endregion

        #region 构造方法

        public TriggerDailyCondition(string id = "") : base(id)
        {
            _type = TaskTriggerConditionType.每天时间段;
        }

        #endregion

        #region 方法

        #region 内部方法

        /// <summary>
        /// 保存数据验证
        /// </summary>
        /// <returns></returns>
        protected override bool SaveValidation()
        {
            var result = base.SaveValidation();
            if (!result)
            {
                return result;
            }
            _beginTime = _beginTime ?? string.Empty;
            _endTime = _endTime ?? string.Empty;
            return true;
        }

        #endregion

        #endregion
    }
}
