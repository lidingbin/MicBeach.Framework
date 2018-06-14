using MicBeach.Develop.DataValidation;
using MicBeach.DataValidation.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MicBeach.Domain.Task.Model;

namespace App.Mapper
{
    /// <summary>
    /// 数据验证配置
    /// </summary>
    public static class DataValidationConfig
    {
        #region 初始化

        public static void Init()
        {
            ValidationManager.StringLength(15, 5, new ValidationField<JobGroup>()
            {
                FieldExpression = g => g.Name,
                ErrorMessage = "jobgroup name is error"
            });
            ValidationManager.NotEqual("abcde", new ValidationField<JobGroup>()
            {
                FieldExpression = c => c.Name,
                ErrorMessage = "can't equals abcde"
            });
        }

        #endregion
    }
}
