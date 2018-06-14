using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation
{
    /// <summary>
    /// 数据验证器
    /// </summary>
    public abstract class DataValidator
    {
        #region 字段

        /// <summary>
        /// 是否验证通过
        /// </summary>
        protected bool _isValid = false;
        /// <summary>
        /// 验证结果
        /// </summary>
        protected VerifyResult _verifyResult = null;
        /// <summary>
        /// 错误消息
        /// </summary>
        protected string _errorMessage = string.Empty;

        #endregion

        #region 属性

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
        }

        /// <summary>
        /// 验证结果
        /// </summary>
        public VerifyResult Result
        {
            get
            {
                return _verifyResult;
            }
        }

        /// <summary>
        /// 默认错误消息
        /// </summary>
        public string DefaultErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 执行验证
        /// </summary>
        /// <param name="value">验证值</param>
        /// <param name="errorMessage">错误消息</param>
        public abstract void Validate(dynamic value,string errorMessage);

        /// <summary>
        /// 生成验证属性
        /// </summary>
        /// <param name="parameter">参数信息</param>
        /// <returns></returns>
        public abstract ValidationAttribute CreateValidationAttribute(ValidationAttributeParameter parameter);

        /// <summary>
        /// 格式化消息
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns></returns>
        protected string FormatMessage(string errorMessage)
        {
            return string.IsNullOrWhiteSpace(errorMessage) ? _errorMessage : errorMessage;
        }

        /// <summary>
        /// 设置验证结果
        /// </summary>
        /// <param name="isValid">验证结果</param>
        /// <param name="message">消息</param>
        protected void SetVerifyResult(bool isValid, string message = "")
        {
            _isValid = isValid;
            _verifyResult = _isValid ? VerifyResult.SuccessResult() : VerifyResult.ErrorResult(FormatMessage(message));
        }

        #endregion
    }
}
