using MicBeach.Util.ExpressionUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// IModify default Implement
    /// </summary>
    internal class ModifyExpression : IModify
    {
        #region fields

        List<object> items = new List<object>();

        #endregion

        #region functions

        /// <summary>
        /// set modify expression
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <param name="fields">fields</param>
        /// <returns>IModify object</returns>
        public IModify Set<T>(params Expression<Func<T, dynamic>>[] fields)
        {
            if (fields == null || fields.Length <= 0)
            {
                return this;
            }
            foreach (var field in fields)
            {
                items.Add(field.Body);
            }
            return this;
        }

        /// <summary>
        /// set modify expression
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Set<T>(Expression<Func<T, dynamic>> field, dynamic value)
        {
            if (value == null)
            {
                return this;
            }
            items.Add(new Tuple<string, dynamic>(ExpressionHelper.GetExpressionPropertyName(field.Body), value));
            return this;
        }

        /// <summary>
        /// set modify value
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Set(string name, dynamic value)
        {
            items.Add(new Tuple<string, dynamic>(name, value));
            return this;
        }

        /// <summary>
        /// calculate with current value then modify
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="calculateOperator">Calculate Operator</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Calculate(string name, CalculateOperator calculateOperator, dynamic value)
        {
            var calculate = new CalculateModify()
            {
                Calculate= calculateOperator,
                Value=value
            };
            items.Add(new Tuple<string, dynamic>(name, calculate));
            return this;
        }

        /// <summary>
        /// calculate with current value then modify
        /// </summary>
        /// <typeparam name="T">Model Type</typeparam>
        /// <param name="field">field</param>
        /// <param name="calculateOperator">Calculate Operator</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Calculate<T>(Expression<Func<T, dynamic>> field, CalculateOperator calculateOperator, dynamic value)
        {
            if (value == null)
            {
                return this;
            }
            return Calculate(ExpressionHelper.GetExpressionPropertyName(field.Body),calculateOperator,value);
        }

        /// <summary>
        /// add value
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Add(string name, dynamic value)
        {
            return Calculate(name, CalculateOperator.Add, value);
        }

        /// <summary>
        /// add value
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Add<T>(Expression<Func<T, dynamic>> field, dynamic value)
        {
            return Add(ExpressionHelper.GetExpressionPropertyName(field.Body), value);
        }

        /// <summary>
        /// subtract value
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Subtract(string name, dynamic value)
        {
            return Calculate(name, CalculateOperator.subtract, value);
        }

        /// <summary>
        /// subtract value
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Subtract<T>(Expression<Func<T, dynamic>> field, dynamic value)
        {
            return Subtract(ExpressionHelper.GetExpressionPropertyName(field.Body), value);
        }

        /// <summary>
        /// multiply value
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Multiply(string name, dynamic value)
        {
            return Calculate(name, CalculateOperator.multiply, value);
        }

        /// <summary>
        /// multiply value
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Multiply<T>(Expression<Func<T, dynamic>> field, dynamic value)
        {
            return Multiply(ExpressionHelper.GetExpressionPropertyName(field.Body), value);
        }

        /// <summary>
        /// divide value
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Divide(string name, dynamic value)
        {
            return Calculate(name, CalculateOperator.divide, value);
        }

        /// <summary>
        /// divide value
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <returns>IModify object</returns>
        public IModify Divide<T>(Expression<Func<T, dynamic>> field, dynamic value)
        {
            return Divide(ExpressionHelper.GetExpressionPropertyName(field.Body), value);
        }

        /// <summary>
        /// get modify values
        /// </summary>
        /// <returns>values</returns>
        public Dictionary<string, dynamic> GetModifyValues()
        {
            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>(items.Count);
            foreach (var item in items)
            {
                Expression exItem = item as Expression;
                if (exItem != null && exItem.NodeType == ExpressionType.MemberAccess)
                {
                    string name = ExpressionHelper.GetExpressionPropertyName(exItem);
                    object value = ExpressionHelper.GetExpressionValue(exItem);
                    values.Add(name, value);
                    continue;
                }
                Tuple<string, dynamic> tupleItem = item as Tuple<string, dynamic>;
                if (tupleItem != null)
                {
                    values.Add(tupleItem.Item1, tupleItem.Item2);
                    continue;
                }
            }
            return values;
        }

        #endregion
    }
}
