using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation
{
    /// <summary>
    /// 生成验证属性参数
    /// </summary>
    public class ValidationAttributeParameter
    {
        #region 属性

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage
        {
            get;set;
        }
        
        /// <summary>
        /// 比较字段
        /// </summary>
        public string OtherProperty
        {
            get;set;
        }

        #endregion
    }
}
