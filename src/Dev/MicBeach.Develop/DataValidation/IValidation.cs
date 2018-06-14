using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation
{
    public interface IValidation
    {
        /// <summary>
        /// 执行验证
        /// </summary>
        /// <param name="obj">验证对象</param>
        /// <returns></returns>
        VerifyResult Validate(dynamic obj);

        /// <summary>
        /// 生成验证属性
        /// </summary>
        /// <returns></returns>
        ValidationAttribute CreateValidationAttribute();
    }
}
