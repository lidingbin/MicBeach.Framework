using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Lazy Member
    /// </summary>
    public class LazyMember<T>
    {
        #region fields

        /// <summary>
        /// value
        /// </summary>
        T _value = default(T);

        /// <summary>
        /// value initor
        /// </summary>
        Lazy<T> _valueInitor = null;

        /// <summary>
        /// value is inited
        /// </summary>
        bool _init = false;

        #endregion

        #region Propertys

        /// <summary>
        /// Get Value
        /// </summary>
        public T Value
        {
            get
            {
                return GetValue();
            }
        }

        /// <summary>
        /// Get Current Value
        /// </summary>
        public T CurrentValue
        {
            get
            {
                return _value;
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// instance a LazyMember<> object
        /// </summary>
        /// <param name="valueLoadFun">load value method</param>
        public LazyMember(Func<T> valueLoadFun)
        {
            _valueInitor = new Lazy<T>(valueLoadFun);
        }

        #endregion

        #region methods

        /// <summary>
        /// get value
        /// </summary>
        /// <returns></returns>
        T GetValue()
        {
            if (_init || _valueInitor.IsValueCreated)
            {
                return _value;
            }
            var newVal = _valueInitor.Value;
            SetValue(newVal, _valueInitor.IsValueCreated);
            return _value;
        }

        /// <summary>
        /// set value
        /// </summary>
        /// <param name="value">new value</param>
        /// <param name="init">set value is inited or not</param>
        public void SetValue(T value, bool init = true)
        {
            _init = init;
            _value = value;
        }

        #endregion
    }
}
