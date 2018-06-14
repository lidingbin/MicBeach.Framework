using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 表达式计划
    /// </summary>
    public class ExpressionTrigger : Trigger
    {
        #region	字段

        /// <summary>
        /// 表达式配置项
        /// </summary>
        protected List<ExpressionItem> _expressionItems;

        #endregion

        #region 构造方法

        /// <summary>
        /// 实例化自定义表达式计划对象
        /// </summary>
        /// <param name="triggerId">编号</param>
        internal ExpressionTrigger(string triggerId = "") : base(triggerId)
        {
            _type = TaskTriggerType.自定义;
        }

        #endregion

        #region	属性

        /// <summary>
        /// 表达式配置项
        /// </summary>
        public List<ExpressionItem> ExpressionItems
        {
            get
            {
                return _expressionItems;
            }
            set
            {
                _expressionItems = value;
            }
        }

        #endregion

        #region 方法

        #region 根据给定的计划对象更新当前信息

        /// <summary>
        /// 根据给定的计划对象更新当前信息
        /// </summary>
        /// <param name="trigger">其它计划信息</param>
        public override void ModifyFromOtherTrigger(Trigger trigger, IEnumerable<string> excludePropertys = null)
        {
            base.ModifyFromOtherTrigger(trigger, excludePropertys);
            if (trigger == null || !(trigger is ExpressionTrigger))
            {
                return;
            }
            var expressionTrigger = trigger as ExpressionTrigger;
            _expressionItems = expressionTrigger.ExpressionItems;
        }

        #endregion

        #endregion
    }
}
