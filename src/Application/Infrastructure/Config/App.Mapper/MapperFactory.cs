using MicBeach.Util.ObjectMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Mapper
{
    public static class MapperFactory
    {
        static IObjectMap objectMapper;
        static MapperFactory()
        {
            //初始化
            objectMapper = new AutoMapMapper();
            objectMapper.Register();
        }

        #region 属性

        /// <summary>
        /// 对象映射转换器
        /// </summary>
        public static IObjectMap ObjectMapper
        {
            get
            {
                return objectMapper;
            }
        }

        #endregion
    }
}
