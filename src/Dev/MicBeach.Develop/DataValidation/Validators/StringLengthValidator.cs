using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation.Validators
{
    /// <summary>
    /// 字符串长度验证
    /// </summary>
    public class StringLengthValidator : DataValidator
    {
        public StringLengthValidator(int maxLength, int minLength = 0)
        {
            MaximumLength = maxLength;
            MinimumLength = minLength;
            _errorMessage = string.Format("字符长度因在{0}到{1}之间", minLength, maxLength);
        }

        #region 属性

        /// <summary>
        /// 最大长度
        /// </summary>
        public int MaximumLength { get; }
        /// <summary>
        /// 最小长度
        /// </summary>
        public int MinimumLength { get; set; }

        #endregion

        #region 方法

        /// <summary>
        /// 执行验证
        /// </summary>
        /// <param name="value">需要验证的值</param>
        /// <param name="errorMessage">错误消息</param>
        public override void Validate(dynamic value, string errorMessage = "")
        {
            EnsureLegalLengths();
            int length = value == null ? 0 : ((string)value).Length;
            _isValid = value == null || (length >= this.MinimumLength && length <= this.MaximumLength);
            SetVerifyResult(_isValid, errorMessage);
        }

        /// <summary>
        /// 内部合法性验证
        /// </summary>
        private void EnsureLegalLengths()
        {
            if (this.MaximumLength < 0)
            {
                throw new InvalidOperationException("MaximumLength Is Less 0");
            }

            if (this.MaximumLength < this.MinimumLength)
            {
                throw new InvalidOperationException("MaximumLength Is Less Than MinimumLength Value");
            }
        }

        /// <summary>
        /// 生成验证属性
        /// </summary>
        /// <returns></returns>
        public override ValidationAttribute CreateValidationAttribute(ValidationAttributeParameter parameter)
        {
            return new StringLengthAttribute(MaximumLength)
            {
                MinimumLength = MinimumLength,
                ErrorMessage = FormatMessage(parameter.ErrorMessage)
            };
        }

        #endregion
    }
}
