using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.IoC
{
    /// <summary>
    /// DI container interface
    /// </summary>
    public interface IDIContainer
    {
        #region Register

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="fromType">source type</param>
        /// <param name="toType">target type</param>
        void RegisterType(Type fromType, Type toType);

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="fromType">source type</param>
        /// <param name="toType">target type</param>
        /// <param name="behaviors">behaviors</param>
        void RegisterType(Type fromType, Type toType, IEnumerable<Type> behaviors);

        #endregion

        #region determine register

        /// <summary>
        ///determine whether has registered the specified type
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <returns>determined value</returns>
        bool IsRegister<T>();

        #endregion

        #region get register

        /// <summary>
        /// get the register type by the specified type
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <returns>register type</returns>
        T Resolve<T>();

        #endregion

    }
}
