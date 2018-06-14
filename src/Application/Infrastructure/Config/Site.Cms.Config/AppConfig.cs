using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.IoC;
using Lee.IoC;
using MicBeach.Util.ObjectMap;
using MicBeach.Util.Security;
using MicBeach.Util;
using Site.Cms.Config;
using App.Mapper;
using App.DBConfig;

namespace Site.Cms.Config
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public static class AppConfig
    {
        #region 初始化

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void Init()
        {
            //依赖注入配置
            ContainerManager.Container = ContainerFactory.Container;
            //对象转换映射
            ObjectMapManager.ObjectMapper = MapperFactory.ObjectMapper;
            //Mvc功能配置
            MvcConfig.Init();
            //数据验证
            DataValidationConfig.Init();
            //显示验证
            DisplayConfig.Init();
            //数据库配置
            DbConfig.Init();
            //对象Id生成初始化
            IdentityKeyConfig.Init();
        }

        #endregion
    }
}
