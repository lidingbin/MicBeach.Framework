using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation
{
    /// <summary>
    /// 验证字段
    /// </summary>
    public class ValidationField<T>
    {
        /// <summary>
        /// 验证字段表达式
        /// </summary>
        public Expression<Func<T, dynamic>> FieldExpression
        {
            get; set;
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage
        {
            get; set;
        }

        /// <summary>
        /// 用于比较的值
        /// </summary>
        internal dynamic CompareValue
        {
            get; set;
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        public bool TipMessage
        {
            get; set;
        }
    }
}
