using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation.Validators
{
    /// <summary>
    /// 比较值
    /// </summary>
    public class CompareOperatorValue
    {
        #region 属性

        /// <summary>
        /// 原值
        /// </summary>
        public dynamic SourceValue
        {
            get;set;
        }

        /// <summary>
        /// 比较值
        /// </summary>
        public dynamic CompareValue
        {
            get;set;
        }

        #endregion
    }
}
