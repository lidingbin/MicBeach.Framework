using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Registration;

namespace Lee.IoC
{
    public class UContainer : IDIContainer
    {
        private static IUnityContainer unityContainer = new UnityContainer();
        private static IDependencyResolver dependencyResolver = new UnityDependencyResolver(unityContainer);

        static UContainer()
        {
            unityContainer.AddNewExtension<Interception>();
        }

        #region 注册类型

        /// <summary>
        /// 注册依赖类型
        /// </summary>
        /// <param name="fromType">源类型</param>
        /// <param name="toType">目标类型</param>
        public void RegisterType(Type fromType, Type toType)
        {
            unityContainer.RegisterType(fromType, toType);
        }

        #endregion

        #region 判断是否注册了相关类型

        /// <summary>
        /// 判断是否注册了相关类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsRegister<T>()
        {
            return unityContainer.IsRegistered<T>();
        }

        #endregion

        #region 获取注册类型的实例对象

        /// <summary>
        /// 获取注册类型的实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }

        public void RegisterType(Type fromType, Type toType, IEnumerable<Type> behaviorTypes)
        {
            List<InjectionMember> members = new List<InjectionMember>();
            if (behaviorTypes == null || behaviorTypes.Count() <= 0)
            {
                RegisterType(fromType, toType);
                return;
            }
            members.Add(new Interceptor<InterfaceInterceptor>());
            foreach (var type in behaviorTypes)
            {
                members.Add(new InterceptionBehavior(type));
            }
            unityContainer.RegisterType(fromType, toType, members.ToArray());
        }

        #endregion

        public IDependencyResolver DependencyResolver
        {
            get { return dependencyResolver; }
        }

    }
}
