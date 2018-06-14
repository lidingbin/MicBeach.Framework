using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Serialize
{
    /// <summary>
    /// json serialize
    /// </summary>
    public static class JsonSerialize
    {
        static readonly IJsonSerializer _jsonSerializer=null;
        static JsonSerialize()
        {
            _jsonSerializer = ContainerManager.Resolve<IJsonSerializer>();
        }

        #region JavaScriptSerializer

        /// <summary>
        /// serialization an object to JSON string by IJsonSerializer
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="obj">data object</param>
        /// <returns>Json string</returns>
        public static string ObjectToJson<T>(T obj)
        {
            if (_jsonSerializer == null)
            {
                throw new Exception("haven't initialized the IJsonSerializer");
            }
            return _jsonSerializer.ObjectToJson(obj);
        }

        /// <summary>
        /// deserialization a json string to an object by IJsonSerializer
        /// </summary>
        /// <param name="json">json string</param>
        /// <returns>data object</returns>
        public static T JsonToObject<T>(string json)
        {
            if (_jsonSerializer == null)
            {
                throw new Exception("haven't initialized the IJsonSerializer");
            }
            return _jsonSerializer.JsonToObject<T>(json);
        }

        #endregion
    }
}
