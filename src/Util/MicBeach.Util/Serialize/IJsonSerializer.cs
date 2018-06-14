using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Util.Serialize
{
    /// <summary>
    /// JSON Serializer
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// serialization an object to a JSON string
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="obj">object</param>
        /// <returns>json string</returns>
        string ObjectToJson<T>(T obj);

        /// <summary>
        /// deserialization a JSON string to an object
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>object</returns>
        T JsonToObject<T>(string json);
    }
}
