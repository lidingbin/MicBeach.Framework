using MicBeach.Develop.CQuery;
using MicBeach.Util.ExpressionUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.Command
{
    /// <summary>
    /// Command Entity
    /// </summary>
    public abstract class CommandEntity<T> where T : CommandEntity<T>
    {
        LifeStatus _lifeStatus = LifeStatus.Stored;//state
        T _oldEntityData = default(T);//stored value
        protected Dictionary<string, dynamic> valueDic = new Dictionary<string, dynamic>();//field values
        private bool _batchReturn = false;//whether is batch returned

        public CommandEntity()
        {
            _oldEntityData = (T)MemberwiseClone();
        }

        #region Propertys

        /// <summary>
        /// Page Count
        /// </summary>
        protected int PagingTotalCount
        {
            get; set;
        }

        /// <summary>
        /// life status
        /// </summary>
        protected LifeStatus LifeStatus
        {
            get
            {
                return _lifeStatus;
            }
        }

        /// <summary>
        /// batch return
        /// </summary>
        protected bool BatchReturn
        {
            get
            {
                return _batchReturn;
            }
            set
            {
                _batchReturn = value;
            }
        }

        /// <summary>
        /// field values
        /// </summary>
        internal Dictionary<string, dynamic> PropertyValues
        {
            get
            {
                return valueDic;
            }
        }

        /// <summary>
        /// Stored Data
        /// </summary>
        private T StoredData
        {
            get
            {
                return _oldEntityData;
            }
            set
            {
                _oldEntityData = value;
            }
        }

        /// <summary>
        /// allow load propertys
        /// </summary>
        protected Dictionary<string, bool> LoadPropertys
        {
            get; set;
        }

        #endregion

        #region methods

        /// <summary>
        /// get has modifed values
        /// </summary>
        /// <returns>values</returns>
        internal Dictionary<string, dynamic> GetModifyValues()
        {
            if (_oldEntityData == null || _oldEntityData.PropertyValues == null)
            {
                return valueDic;
            }
            Dictionary<string, dynamic> modifyValues = new Dictionary<string, dynamic>(valueDic.Count);
            var oldValues = _oldEntityData.PropertyValues;
            var versionField = QueryConfig.GetVersionField(typeof(T));
            foreach (var valueItem in valueDic)
            {
                if (valueItem.Key != versionField && (!oldValues.ContainsKey(valueItem.Key) || oldValues[valueItem.Key] != valueItem.Value))
                {
                    modifyValues.Add(valueItem.Key, valueItem.Value);
                }
            }
            return modifyValues;
        }

        /// <summary>
        /// get object primary key value
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, dynamic> GetPrimaryKeyValues()
        {
            var primaryKeys = QueryConfig.GetPrimaryKeys(typeof(T));
            if (primaryKeys == null || primaryKeys.Count <= 0)
            {
                return new Dictionary<string, dynamic>(0);
            }
            Dictionary<string, dynamic> values = new Dictionary<string, dynamic>(primaryKeys.Count);
            foreach (var key in primaryKeys)
            {
                if (valueDic.ContainsKey(key))
                {
                    values.Add(key, valueDic[key]);
                }
            }
            return values;
        }

        /// <summary>
        /// compare two objects determine whether is equal
        /// </summary>
        /// <param name="obj">compare object</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            CommandEntity<T> targetObj = obj as CommandEntity<T>;
            if (targetObj == null)
            {
                return false;
            }
            var myValues = GetPrimaryKeyValues();
            var targetValues = targetObj.GetPrimaryKeyValues();
            if (myValues == null || targetValues == null || myValues.Count != targetValues.Count)
            {
                return false;
            }
            bool equal = true;
            foreach (var myVal in myValues)
            {
                equal = equal && targetValues.ContainsKey(myVal.Key) && myVal.Value == targetValues[myVal.Key];
                if (!equal)
                {
                    break;
                }
            }
            return equal;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// get primary keys with cache
        /// </summary>
        /// <returns></returns>
        public string GetPrimaryCacheKey(bool includeObjectName = true)
        {
            SortedSet<string> primaryKeys = QueryConfig.GetPrimaryKeys(typeof(T));
            return GenerateCacheKey(primaryKeys, includeObjectName);
        }

        /// <summary>
        /// get cache keys
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="includeObjectName">whether include object name</param>
        /// <param name="propertys"></param>
        /// <returns></returns>
        public string GetCacheKey(bool includeObjectName = true, params Expression<Func<T, dynamic>>[] propertys)
        {
            if (propertys == null)
            {
                return string.Empty;
            }
            return GetCacheKey(includeObjectName, propertys.Select(c => ExpressionHelper.GetExpressionPropertyName(c.Body)).ToArray());
        }

        /// <summary>
        /// get chache keys
        /// </summary>
        /// <param name="includeObjectName">whether include object name</param>
        /// <param name="keys">cache keys</param>
        /// <returns></returns>
        public string GetCacheKey(bool includeObjectName = true, params string[] keys)
        {
            if (keys == null || keys.Length <= 0)
            {
                return string.Empty;
            }
            SortedSet<string> sortedKeys = new SortedSet<string>();
            foreach (string key in keys)
            {
                sortedKeys.Add(key);
            }
            return GenerateCacheKey(keys, includeObjectName);
        }

        /// <summary>
        /// get cache keys
        /// </summary>
        /// <param name="keyAndValues">key and values</param>
        /// <param name="includeObjectName">whether include object name</param>
        /// <returns></returns>
        public static string GetCacheKey(IDictionary<string, dynamic> keyAndValues, bool includeObjectName = true)
        {
            List<string> keys = new List<string>();
            if (includeObjectName)
            {
                Type type = typeof(T);
                string objectName = QueryConfig.GetObjectName(type);
                if (!string.IsNullOrWhiteSpace(objectName))
                {
                    keys.Add(objectName);
                }
            }
            if (keyAndValues != null && keyAndValues.Count > 0)
            {
                SortedDictionary<string, dynamic> sortedValues = new SortedDictionary<string, dynamic>();
                foreach (var valItem in keyAndValues)
                {
                    sortedValues.Add(valItem.Key, valItem.Value);
                }
                foreach (var sortValItem in sortedValues)
                {
                    keys.Add(string.Format("{0}${1}", sortValItem.Key, sortValItem.Value));
                }
            }
            return string.Join(":", keys);
        }

        /// <summary>
        /// generate cache key
        /// </summary>
        /// <param name="keys">keys</param>
        /// <param name="includeObjectName">whether include object name</param>
        /// <returns></returns>
        string GenerateCacheKey(IEnumerable<string> keys, bool includeObjectName = true)
        {
            List<string> keyValues = new List<string>();
            var type = typeof(T);
            if (includeObjectName)
            {
                string objectName = QueryConfig.GetObjectName(type);
                if (!string.IsNullOrWhiteSpace(objectName))
                {
                    keyValues.Add(objectName);
                }
            }
            if (keys != null)
            {
                foreach (string key in keys)
                {
                    keyValues.Add(string.Format("{0}${1}", key, valueDic[key]));
                }
            }
            return string.Join(":", keyValues);
        }

        /// <summary>
        /// get paging count
        /// </summary>
        /// <returns></returns>
        public int GetPagingTotalCount()
        {
            return PagingTotalCount;
        }

        /// <summary>
        /// get object name
        /// </summary>
        /// <returns></returns>
        public static string GetObjectName()
        {
            Type type = typeof(T);
            return QueryConfig.GetObjectName(type);
        }

        /// <summary>
        /// get property name
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        public dynamic GetPropertyValue(string propertyName)
        {
            if (valueDic.ContainsKey(propertyName))
            {
                return valueDic[propertyName];
            }
            throw new Exception("error property");
        }

        /// <summary>
        /// batch init
        /// </summary>
        /// <param name="query">query object</param>
        public void BatchInit(IQuery query)
        {
            _batchReturn = true;
            if (query != null)
            {
                LoadPropertys = query.LoadPropertys;
            }
        }

        /// <summary>
        /// single init
        /// </summary>
        /// <param name="query">query object</param>
        public void SingleInit(IQuery query)
        {
            _batchReturn = false;
            if (query != null)
            {
                LoadPropertys = query.LoadPropertys;
            }
        }

        #endregion
    }
}
