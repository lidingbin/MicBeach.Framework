using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation
{
    /// <summary>
    /// 验证结果
    /// </summary>
    public class VerifyResult
    {
        #region 属性

        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool Success
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
        /// 验证字段名称
        /// </summary>
        public string FieldName
        {
            get;set;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <returns></returns>
        public static VerifyResult ErrorResult(string errorMessage = "")
        {
            return new VerifyResult()
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }

        /// <summary>
        /// 正确结果
        /// </summary>
        /// <param name="successMessage">正确信息</param>
        /// <returns></returns>
        public static VerifyResult SuccessResult(string successMessage = "")
        {
            return new VerifyResult()
            {
                Success = true,
                ErrorMessage = successMessage
            };
        }

        #endregion
    }
}
