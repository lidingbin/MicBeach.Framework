using MicBeach.DataValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Site.Cms.Config
{
    /// <summary>
    /// Mvc自定义配置
    /// </summary>
    public static class MvcConfig
    {
        public static void Init()
        {
            #region 自定义MVC数据验证配置

            ModelValidatorProviders.Providers.Add(new CustomModelValidatorProvider());//自定义数据验证
            int i = 0;
            foreach (var provider in ModelValidatorProviders.Providers)
            {
                if (provider.GetType().FullName == typeof(ClientDataTypeModelValidatorProvider).FullName)
                {
                    break;
                }
                i++;
            }
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;//去除自动添加必填验证
            ModelValidatorProviders.Providers.RemoveAt(i);//移除默认客户端验证

            #endregion

            #region 自定义MVC显示

            ModelMetadataProviders.Current = new CustomModelMetadataProvider();//自定义显示名称

            #endregion
        }
    }
}
