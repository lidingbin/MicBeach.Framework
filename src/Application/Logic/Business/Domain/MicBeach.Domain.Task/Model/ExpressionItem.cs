using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.CTask;

namespace MicBeach.Domain.Task.Model
{
    /// <summary>
    /// 表达式选项
    /// </summary>
    public class ExpressionItem
    {
        #region	字段

        /// <summary>
        /// 表达式配置项
        /// </summary>
        protected TaskTriggerExpressionItem _option;

        /// <summary>
        /// 值类型
        /// </summary>
        protected TaskTriggerExpressionItemConfigType _valueType;

        /// <summary>
        /// 开始值
        /// </summary>
        protected int _beginValue;

        /// <summary>
        /// 结束值
        /// </summary>
        protected int _endValue;

        /// <summary>
        /// 集合值
        /// </summary>
        protected List<string> _arrayValue;

        #endregion

        #region	属性

        /// <summary>
        /// 表达式配置项
        /// </summary>
        public TaskTriggerExpressionItem Option
        {
            get
            {
                return _option;
            }
            protected set
            {
                _option = value;
            }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public TaskTriggerExpressionItemConfigType ValueType
        {
            get
            {
                return _valueType;
            }
            protected set
            {
                _valueType = value;
            }
        }

        /// <summary>
        /// 开始值
        /// </summary>
        public int BeginValue
        {
            get
            {
                return _beginValue;
            }
            protected set
            {
                _beginValue = value;
            }
        }

        /// <summary>
        /// 结束值
        /// </summary>
        public int EndValue
        {
            get
            {
                return _endValue;
            }
            protected set
            {
                _endValue = value;
            }
        }

        /// <summary>
        /// 集合值
        /// </summary>
        public List<string> ArrayValue
        {
            get
            {
                return _arrayValue;
            }
            protected set
            {
                _arrayValue = value;
            }
        }

        #endregion
    }
}
