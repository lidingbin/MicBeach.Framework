using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    public class Criteria : IQueryItem
    {
        CriteriaOperator _operator;
        bool _calculateValue = false;//是否计算过真实值
        dynamic _realValue = null;//条件表达式真实值

        /// <summary>
        /// 初始化条件表达式
        /// </summary>
        /// <param name="name">field name</param>
        /// <param name="operator">conditional operator</param>
        /// <param name="value">value</param>
        private Criteria(string name, CriteriaOperator @operator, dynamic value)
        {
            Name = name;
            _operator = @operator;
            Value = value;
        }

        #region 属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// 运算符
        /// </summary>
        public CriteriaOperator Operator
        {
            get
            {
                return _operator;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public dynamic Value
        {
            get;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取条件最终的参数值
        /// </summary>
        /// <returns></returns>
        public dynamic GetCriteriaRealValue()
        {
            if (_calculateValue)
            {
                return _realValue;
            }
            Expression valueExpression = Value as Expression;
            if (valueExpression != null)
            {
                _realValue = GetExpressionValue(valueExpression);
            }
            else
            {
                _realValue = Value;
            }
            _calculateValue = true;
            return _realValue;
        }

        /// <summary>
        /// 计算表达式参数值
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <returns></returns>
        dynamic GetExpressionValue(Expression valueExpression)
        {
            if (valueExpression == null)
            {
                return null;
            }
            dynamic value = null;
            switch (valueExpression.NodeType)
            {
                case ExpressionType.Constant:
                    value = ((ConstantExpression)valueExpression).Value;
                    break;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.ArrayIndex:
                case ExpressionType.ArrayLength:
                case ExpressionType.Call:
                case ExpressionType.Coalesce:
                case ExpressionType.Conditional:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Decrement:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Increment:
                case ExpressionType.Invoke:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.New:
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                case ExpressionType.OnesComplement:
                case ExpressionType.Or:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.MemberAccess:
                    value = Expression.Lambda(valueExpression).Compile().DynamicInvoke();
                    break;
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// 创建一个新的条件表达式
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="operator">operator</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static Criteria CreateNewCriteria(string name, CriteriaOperator @operator, dynamic value)
        {
            return new Criteria(name, @operator, value);
        }

        #endregion
    }
}
