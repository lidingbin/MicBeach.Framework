using MicBeach.Util.IoC;
using MicBeach.Util.ObjectMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicBeach.Util.Serialize;

namespace MicBeach.Util.Extension
{
    /// <summary>
    /// object extension methods
    /// </summary>
    public static class ObjectExtension
    {
        #region generate a dictionary by an object

        /// <summary>
        /// generate a dictionary by an object
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>dictionary value</returns>
        public static Dictionary<string, object> ObjectToDcitionary(this object value)
        {
            if (value == null)
            {
                return new Dictionary<string, object>(0);
            }
            PropertyDescriptorCollection nowPropertyCollection = TypeDescriptor.GetProperties(value);
            Dictionary<string, object> valueDictionary = new Dictionary<string, object>(nowPropertyCollection.Count);
            foreach (PropertyDescriptor ps in nowPropertyCollection)
            {
                valueDictionary.Add(ps.Name, ps.GetValue(value));
            }
            return valueDictionary;
        }

        /// <summary>
        /// generate a dictionary by an object
        /// </summary>
        /// <param name="value">object</param>
        /// <returns>dictionary value</returns>
        public static Dictionary<string, string> ObjectToStringDcitionary(this object value)
        {
            if (value == null)
            {
                return new Dictionary<string, string>(0);
            }
            PropertyDescriptorCollection nowPropertyCollection = TypeDescriptor.GetProperties(value);
            Dictionary<string, string> valueDictionary = new Dictionary<string, string>(nowPropertyCollection.Count);
            foreach (PropertyDescriptor ps in nowPropertyCollection)
            {
                valueDictionary.Add(ps.Name, ps.GetValue(value).ToString());
            }
            return valueDictionary;
        }

        #endregion

        #region get a string by an object

        /// <summary>
        /// get a string by an object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>string value</returns>
        public static string TooString(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return obj.ToString();
        }

        #endregion

        #region object map

        /// <summary>
        /// map an object to an other object
        /// </summary>
        /// <typeparam name="T">target data type</typeparam>
        /// <param name="source">source object</param>
        /// <returns>target object</returns>
        public static T MapTo<T>(this object source)
        {
            return ObjectMapManager.ObjectMapper.MapTo<T>(source);
        }

        #endregion

        #region get an register type instance by ContainerManager

        /// <summary>
        /// get an register type instance by ContainerManager
        /// </summary>
        /// <typeparam name="T">register type</typeparam>
        /// <param name="sourceObj">source object</param>
        /// <returns>register type instance</returns>
        public static T Instance<T>(this object sourceObj)
        {
            return ContainerManager.Resolve<T>();
        }

        #endregion

        #region object deep clone

        /// <summary>
        /// object deep clone
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="sourceObj">object</param>
        /// <returns>new object</returns>
        public static T DeepClone<T>(this T sourceObj)
        {
            if (sourceObj == null)
            {
                return default(T);
            }
            var objectString = JsonSerialize.ObjectToJson<T>(sourceObj);
            return JsonSerialize.JsonToObject<T>(objectString);
        }

        #endregion
    }
}
