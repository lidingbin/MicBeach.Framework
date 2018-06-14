
using MicBeach.Business.Task;
using MicBeach.BusinessInterface.Task;
using MicBeach.DataAccess.Task;
using MicBeach.DataAccessInterface.Task;
using MicBeach.Domain.Task.Repository;
using MicBeach.Repository.Task;
using MicBeach.Serialize.Json.JsonNet;
using MicBeach.Service.Task;
using MicBeach.ServiceInterface.Task;
using MicBeach.Util.IoC;
using MicBeach.Util.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lee.IoC
{
    /// <summary>
    /// IoC控制容器工厂
    /// </summary>
    public class ContainerFactory
    {
        static IDIContainer container;//依赖注入容器

        #region 构造方法

        static ContainerFactory()
        {
            UContainer ucontainer = new UContainer();
            container = ucontainer;
            RegisterTypes();
            DependencyResolver.SetResolver(ucontainer.DependencyResolver);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取依赖注入容器
        /// </summary>
        public static IDIContainer Container
        {
            get
            {
                return container;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 注册类型
        /// </summary>
        static void RegisterTypes()
        {
            List<Type> types = new List<Type>();

            #region Task

            types.AddRange(typeof(IJobGroupDataAccess).Assembly.GetTypes());
            types.AddRange(typeof(JobGroupDataAccess).Assembly.GetTypes());
            types.AddRange(typeof(IJobGroupBusiness).Assembly.GetTypes());
            types.AddRange(typeof(JobGroupBusiness).Assembly.GetTypes());
            types.AddRange(typeof(IJobGroupRepository).Assembly.GetTypes());
            types.AddRange(typeof(JobGroupRepository).Assembly.GetTypes());
            types.AddRange(typeof(IJobGroupService).Assembly.GetTypes());
            types.AddRange(typeof(JobGroupService).Assembly.GetTypes());

            #endregion

            foreach (Type type in types)
            {
                string typeName = type.Name;
                if (!typeName.StartsWith("I"))
                {
                    continue;
                }
                if (typeName.EndsWith("Service") || typeName.EndsWith("Business") || typeName.EndsWith("DbAccess") || typeName.EndsWith("Repository"))
                {
                    Type realType = types.FirstOrDefault(t => t.Name != type.Name && !t.IsInterface && type.IsAssignableFrom(t));
                    if (realType != null)
                    {
                        List<Type> behaviors = new List<Type>();
                        container.RegisterType(type, realType, behaviors);
                    }
                }
                if (typeName.EndsWith("DataAccess"))
                {
                    List<Type> relateTypes = types.Where(t => t.Name != type.Name && !t.IsInterface && type.IsAssignableFrom(t)).ToList();
                    if (relateTypes != null && relateTypes.Count > 0)
                    {
                        Type providerType = relateTypes.FirstOrDefault(c => c.Name.EndsWith("Cache"));
                        providerType = providerType ?? relateTypes.First();
                        container.RegisterType(type, providerType);
                    }
                }
            }
            //container.RegisterType(typeof(IEmailEngine), typeof(NetEmailEngine));
            //container.RegisterType(typeof(ISMSEngine), typeof(STSMSEngine));
            //container.RegisterType(typeof(ICacheAction), typeof(DefaultCacheAction));
            container.RegisterType(typeof(IJsonSerializer), typeof(JsonNetSerializer));
        }

        #endregion
    }
}
