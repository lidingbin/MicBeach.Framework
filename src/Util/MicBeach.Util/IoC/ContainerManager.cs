using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.IoC
{
    /// <summary>
    /// DI container manager
    /// </summary>
    public static class ContainerManager
    {
        static IDIContainer container = null;

        #region Propertys

        /// <summary>
        /// DI Container
        /// </summary>
        public static IDIContainer Container
        {
            get
            {
                return container;
            }
            set
            {
                container = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// determine whether register the specified type
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <returns>registered or not</returns>
        public static bool IsRegister<T>()
        {
            return container.IsRegister<T>();
        }

        /// <summary>
        /// resolve the specified type
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <returns>resolve type</returns>
        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        #endregion
    }
}
