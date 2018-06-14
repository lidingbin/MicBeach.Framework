using MicBeach.Util.ExpressionUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// Query Config
    /// </summary>
    public static class QueryConfig
    {
        #region fields

        static Dictionary<string, SortedSet<string>> _primaryKeys = new Dictionary<string, SortedSet<string>>();//primary key field
        static Dictionary<string, SortedSet<string>> _cacheKeys = new Dictionary<string, SortedSet<string>>();//cache Keys;
        static Dictionary<string, string> _objectNames = new Dictionary<string, string>();//object name(table name)
        static Dictionary<string, string> _versionFields = new Dictionary<string, string>();//version field
        static Dictionary<string, Dictionary<string, string>> _foreignKeys = new Dictionary<string, Dictionary<string, string>>();//foreign keys
        static Dictionary<string, string> _lastRefreshDateFields = new Dictionary<string, string>();//last refresh date fields

        #endregion

        #region Method

        #region Set Primary Key

        /// <summary>
        /// Set Primary Key
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="keyNames">key name</param>
        public static void SetPrimaryKey(Type type, params string[] keyNames)
        {
            SetPrimaryKey(type.FullName, keyNames);
        }

        /// <summary>
        /// Set Primary Key
        /// </summary>
        /// <param name="typeFullName">type full name</param>
        /// <param name="keyNames">key name</param>
        public static void SetPrimaryKey(string typeFullName, params string[] keyNames)
        {
            if (string.IsNullOrWhiteSpace(typeFullName) || keyNames == null || keyNames.Length <= 0)
            {
                return;
            }
            lock (_primaryKeys)
            {
                SortedSet<string> keySets = null;
                if (_primaryKeys.ContainsKey(typeFullName))
                {
                    keySets = _primaryKeys[typeFullName];
                }
                else
                {
                    keySets = new SortedSet<string>();
                    _primaryKeys.Add(typeFullName, keySets);
                }
                foreach (string key in keyNames)
                {
                    keySets.Add(key);
                }
            }
        }

        /// <summary>
        /// Set Primary Key
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="fields">field expression</param>
        public static void SetPrimaryKey<T>(params Expression<Func<T, dynamic>>[] fields)
        {
            if (fields == null)
            {
                return;
            }
            SetPrimaryKey(typeof(T), fields.Select(c => ExpressionHelper.GetExpressionPropertyName(c.Body)).ToArray());
        }

        #endregion

        #region Get Primary Key

        /// <summary>
        /// Get Primary Key
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <returns></returns>
        public static SortedSet<string> GetPrimaryKeys(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                return null;
            }
            if (_primaryKeys.ContainsKey(typeFullName))
            {
                return _primaryKeys[typeFullName];
            }
            return null;
        }

        /// <summary>
        /// Get Primary Key
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static SortedSet<string> GetPrimaryKeys(Type type)
        {
            return GetPrimaryKeys(type.FullName);
        }

        #endregion

        #region Set ObjectName

        /// <summary>
        /// Set ObjectName
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <param name="objectName">object name</param>
        public static void SetObjectName(string typeFullName, string objectName)
        {
            lock (_objectNames)
            {
                if (_objectNames.ContainsKey(typeFullName))
                {
                    _objectNames[typeFullName] = objectName;
                }
                else
                {
                    _objectNames.Add(typeFullName, objectName);
                }
            }
        }

        /// <summary>
        /// Set ObjectName
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="objectName">object name</param>
        public static void SetObjectName(Type type, string objectName)
        {
            SetObjectName(type.FullName, objectName);
        }

        /// <summary>
        /// set ObjectName
        /// </summary>
        /// <param name="objectName">object name</param>
        /// <param name="types">type</param>
        public static void SetObjectName(string objectName, params Type[] types)
        {
            if (types == null || types.Length <= 0)
            {
                return;
            }
            foreach (var type in types)
            {
                SetObjectName(type, objectName);
            }
        }

        /// <summary>
        /// Set ObjectName
        /// </summary>
        /// <param name="objectName">object name</param>
        /// <param name="typeFullNames">type names</param>
        public static void SetObjectName(string objectName, params string[] typeFullNames)
        {
            if (typeFullNames == null || typeFullNames.Length <= 0)
            {
                return;
            }
            foreach (var typeName in typeFullNames)
            {
                SetObjectName(typeName, objectName);
            }
        }

        #endregion

        #region Get ObjectName

        /// <summary>
        /// Get ObjectName
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <returns></returns>
        public static string GetObjectName(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                return string.Empty;
            }
            if (_objectNames.ContainsKey(typeFullName))
            {
                return _objectNames[typeFullName];
            }
            return string.Empty;
        }

        /// <summary>
        /// Get ObjectName
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static string GetObjectName(Type type)
        {
            return GetObjectName(type.FullName);
        }

        #endregion

        #region Set Version Field

        /// <summary>
        /// Set Version Field
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <param name="fieldName">field name</param>
        public static void SetVersionField(string typeFullName, string fieldName)
        {
            lock (_versionFields)
            {
                if (_versionFields.ContainsKey(typeFullName))
                {
                    _versionFields[typeFullName] = fieldName;
                }
                else
                {
                    _versionFields.Add(typeFullName, fieldName);
                }
            }
        }

        /// <summary>
        /// Set Version Field
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="fieldName">field name</param>
        public static void SetVersionField(Type type, string fieldName)
        {
            SetVersionField(type.FullName, fieldName);
        }

        /// <summary>
        /// Set Version Field
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="field">field expression</param>
        public static void SetVersionField<T>(Expression<Func<T, dynamic>> field)
        {
            if (field == null)
            {
                return;
            }
            SetVersionField(typeof(T), ExpressionHelper.GetExpressionPropertyName(field.Body));
        }

        #endregion

        #region Get Version Field

        /// <summary>
        /// Get Version Field
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <returns></returns>
        public static string GetVersionField(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName) || !_versionFields.ContainsKey(typeFullName))
            {
                return string.Empty;
            }
            return _versionFields[typeFullName];
        }

        /// <summary>
        /// Get Version Field
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static string GetVersionField(Type type)
        {
            return GetVersionField(type.FullName);
        }

        #endregion

        #region Set Cache Key

        /// <summary>
        /// Set Cache Key
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="keyNames">key names</param>
        public static void SetCacheKey(Type type, params string[] keyNames)
        {
            SetCacheKey(type.FullName, keyNames);
        }

        /// <summary>
        /// Set Cache Key
        /// </summary>
        /// <param name="typeFullName">type full name</param>
        /// <param name="keyNames">key names</param>
        public static void SetCacheKey(string typeFullName, params string[] keyNames)
        {
            if (string.IsNullOrWhiteSpace(typeFullName) || keyNames == null || keyNames.Length <= 0)
            {
                return;
            }
            lock (_cacheKeys)
            {
                SortedSet<string> keySets = null;
                if (_cacheKeys.ContainsKey(typeFullName))
                {
                    keySets = _cacheKeys[typeFullName];
                }
                else
                {
                    keySets = new SortedSet<string>();
                    _cacheKeys.Add(typeFullName, keySets);
                }
                foreach (string keyName in keyNames)
                {
                    keySets.Add(keyName);
                }
            }
        }

        /// <summary>
        /// Set Cache Key
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="fields">field expression</param>
        public static void SetCacheKey<T>(params Expression<Func<T, dynamic>>[] fields)
        {
            if (fields == null)
            {
                return;
            }
            SetCacheKey(typeof(T), fields.Select(c => ExpressionHelper.GetExpressionPropertyName(c.Body)).ToArray());
        }

        #endregion

        #region Get Cache Key

        /// <summary>
        /// Get Cache Key
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <returns></returns>
        public static SortedSet<string> GetCacheKeys(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                return null;
            }
            if (_cacheKeys.ContainsKey(typeFullName))
            {
                return _cacheKeys[typeFullName];
            }
            return null;
        }

        /// <summary>
        /// Get Cache Key
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static SortedSet<string> GetCacheKeys(Type type)
        {
            return GetCacheKeys(type.FullName);
        }

        #endregion

        #region Set Foreign Key

        /// <summary>
        /// Set Foreign keys
        /// </summary>
        /// <param name="typeFullName">typeFullName</param>
        /// <param name="foreignKeys">foreignKeys(key is child field name ,value parent field name)</param>
        public static void SetForeignKey(string typeFullName, Dictionary<string, string> foreignKeys)
        {
            if (string.IsNullOrWhiteSpace(typeFullName) || foreignKeys == null || foreignKeys.Count <= 0)
            {
                return;
            }
            lock (_foreignKeys)
            {
                if (_foreignKeys.ContainsKey(typeFullName))
                {
                    _foreignKeys[typeFullName] = foreignKeys;
                }
                else
                {
                    _foreignKeys.Add(typeFullName, foreignKeys);
                }
            }
        }

        /// <summary>
        /// Set Foreign keys
        /// </summary>
        /// <param name="type">data type</param>
        /// <param name="foreignKeys">foreignKeys(key is child field name ,value parent field name)</param>
        public static void SetForeignKey(Type type, Dictionary<string, string> foreignKeys)
        {
            string typeFullName = type.FullName;
            SetForeignKey(typeFullName, foreignKeys);
        }

        /// <summary>
        /// Set Foreign keys
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="foreignKeys">foreignKeys(key is child field name ,value parent field name)</param>
        public static void SetForeignKey<T>(Dictionary<string, string> foreignKeys)
        {
            var type = typeof(T);
            SetForeignKey(type, foreignKeys);
        }

        #endregion

        #region Get Foreign Key
        /// <summary>
        /// get foreign key
        /// </summary>
        /// <param name="typeFullName">typeFullName</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetForeignKeys(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                return new Dictionary<string, string>(0);
            }
            if (_foreignKeys.ContainsKey(typeFullName))
            {
                return _foreignKeys[typeFullName];
            }
            return new Dictionary<string, string>(0);
        }

        /// <summary>
        /// get foreign key
        /// </summary>
        /// <param name="type">data type</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetForeignKeys(Type type)
        {
            string typeFullName = type.FullName;
            return GetForeignKeys(typeFullName);
        }

        /// <summary>
        /// get foreign key
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetForeignKeys<T>()
        {
            Type type = typeof(T);
            return GetForeignKeys(type);
        }

        #endregion

        #region Set RefreshDate Field

        /// <summary>
        /// Set RefreshDate Field
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <param name="fieldName">field name</param>
        public static void SetRefreshDateField(string typeFullName, string fieldName)
        {
            lock (_lastRefreshDateFields)
            {
                if (_lastRefreshDateFields.ContainsKey(typeFullName))
                {
                    _lastRefreshDateFields[typeFullName] = fieldName;
                }
                else
                {
                    _lastRefreshDateFields.Add(typeFullName, fieldName);
                }
            }
        }

        /// <summary>
        /// Set RefreshDate Field
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="fieldName">field name</param>
        public static void SetRefreshDateField(Type type, string fieldName)
        {
            SetRefreshDateField(type.FullName, fieldName);
        }

        /// <summary>
        /// Set RefreshDate Field
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="field">field expression</param>
        public static void SetRefreshDateField<T>(Expression<Func<T, dynamic>> field)
        {
            if (field == null)
            {
                return;
            }
            SetRefreshDateField(typeof(T), ExpressionHelper.GetExpressionPropertyName(field.Body));
        }

        #endregion

        #region Get RefreshDate Field

        /// <summary>
        /// Get RefreshDate Field
        /// </summary>
        /// <param name="typeFullName">type name</param>
        /// <returns></returns>
        public static string GetRefreshDateField(string typeFullName)
        {
            if (string.IsNullOrWhiteSpace(typeFullName) || !_lastRefreshDateFields.ContainsKey(typeFullName))
            {
                return string.Empty;
            }
            return _lastRefreshDateFields[typeFullName];
        }

        /// <summary>
        /// Get RefreshDate Field
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static string GetRefreshDateField(Type type)
        {
            return GetRefreshDateField(type.FullName);
        }

        #endregion

        #endregion
    }
}
