using MicBeach.CTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 自定义表达式条件
    /// </summary>
    public class TriggerExpressionCondition : TriggerCondition
    {
        #region	字段

        /// <summary>
        /// 表达式配置项
        /// </summary>
        protected List<ExpressionItem> _expressionItems;

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

        #region 构造方法

        public TriggerExpressionCondition(string id = ""):base(id)
        {
            _type = TaskTriggerConditionType.自定义;
        }

        #endregion
    }
}
