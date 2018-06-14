using MicBeach.Util.Paging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// 查询对象接口
    /// </summary>
    public interface IQuery : IQueryItem
    {
        #region 属性

        /// <summary>
        /// 查询对象名称
        /// </summary>
        string ObjectName
        {
            get;
        }

        /// <summary>
        /// 条件表达式
        /// </summary>
        List<Tuple<QueryOperator, IQueryItem>> Criterias
        {
            get;
        }

        /// <summary>
        /// 要查询的值（优先级大于不查询的值）
        /// </summary>
        List<string> QueryFields
        {
            get;
        }

        /// <summary>
        /// 不查询的值
        /// </summary>
        List<string> NotQueryFields
        {
            get;
        }

        /// <summary>
        /// 分页信息
        /// </summary>
        PagingFilter PagingInfo
        {
            get; set;
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        int QuerySize
        {
            get; set;
        }

        /// <summary>
        /// 查询语句
        /// </summary>
        string QueryText
        {
            get;
        }

        /// <summary>
        /// 查询语句参数
        /// </summary>
        dynamic QueryTextParameters
        {
            get;
        }

        /// <summary>
        /// 查询命令类型
        /// </summary>
        QueryCommandType QueryType
        {
            get;
        }

        /// <summary>
        /// orders
        /// </summary>
        List<OrderCriteria> Orders
        {
            get;
        }

        /// <summary>
        /// Allow Load Propertys
        /// </summary>
        Dictionary<string, bool> LoadPropertys
        {
            get;
        }

        #endregion

        #region 方法

        #region And

        /// <summary>
        /// Connect a condition with 'and'
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="criteria">criteria</param>
        /// <returns>return newest instance</returns>
        IQuery And<T>(Expression<Func<T, bool>> criteria) where T : IQueryModel<T>;

        /// <summary>
        /// Connect a condition with 'and'
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <typeparam name="TProperty">field type</typeparam>
        /// <param name="field">field expression</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <returns>return newest instance</returns>
        IQuery And<T>(Expression<Func<T, dynamic>> field, CriteriaOperator @operator, dynamic value) where T : IQueryModel<T>;

        /// <summary>
        /// Connect a condition with 'and'
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="eachFieldConnectOperator">each field connect operator</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <param name="fields">field type</param>
        /// <returns>return newest instance</returns>
        IQuery And<T>(QueryOperator eachFieldConnectOperator, CriteriaOperator @operator, dynamic value, params Expression<Func<T, dynamic>>[] fields) where T : IQueryModel<T>;

        /// <summary>
        /// Connect a condition with 'and'
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <returns>return newest instance</returns>
        IQuery And(string fieldName, CriteriaOperator @operator, dynamic value);

        /// <summary>
        /// Connect a condition with 'and'
        /// </summary>
        /// <param name="eachFieldConnectOperator">each field connect operator</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <param name="fieldNames">field collection</param>
        /// <returns>return newest instance</returns>
        IQuery And(QueryOperator eachFieldConnectOperator, CriteriaOperator @operator, dynamic value, params string[] fieldNames);

        /// <summary>
        /// Connect a condition with 'and'
        /// </summary>
        /// <param name="subQuery">sub query instance</param>
        /// <returns>return newest instance</returns>
        IQuery And(IQuery subQuery);

        #endregion

        #region OR

        /// <summary>
        /// Connect a condition with 'or'
        /// </summary>
        /// <typeparam name="T">datatype</typeparam>
        /// <param name="criteria">criteria</param>
        /// <returns>return newest instance</returns>
        IQuery Or<T>(Expression<Func<T, bool>> criteria) where T : IQueryModel<T>;

        /// <summary>
        /// Connect a condition with 'or'
        /// </summary>
        /// <typeparam name="T">datatype</typeparam>
        /// <typeparam name="TProperty">field type</typeparam>
        /// <param name="field">field</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <returns>return newest instance</returns>
        IQuery Or<T>(Expression<Func<T, dynamic>> field, CriteriaOperator @operator, dynamic value) where T : IQueryModel<T>;

        /// <summary>
        /// Connect a condition with 'or'
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="eachFieldConnectOperator">each field connect operator</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <param name="fields">field type</param>
        /// <returns>return newest instance</returns>
        IQuery Or<T>(QueryOperator eachFieldConnectOperator, CriteriaOperator @operator, dynamic value, params Expression<Func<T, dynamic>>[] fields) where T : IQueryModel<T>;

        /// <summary>
        /// Connect a condition with 'or'
        /// </summary>
        /// <param name="eachFieldConnectOperator">each field connect operator</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <param name="fieldNames">field collection</param>
        /// <returns>return newest instance</returns>
        IQuery Or(QueryOperator eachFieldConnectOperator, CriteriaOperator @operator, dynamic value, params string[] fieldNames);

        /// <summary>
        /// Connect a condition with 'or'
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="operator">condition operator</param>
        /// <param name="value">value</param>
        /// <returns>return newest instance</returns>
        IQuery Or(string fieldName, CriteriaOperator @operator, dynamic value);

        /// <summary>
        /// Connect a condition with 'or'
        /// </summary>
        /// <param name="subQuery">sub query instance</param>
        /// <returns>return newest instance</returns>
        IQuery Or(IQuery subQuery);

        #endregion

        #region Equal

        /// <summary>
        /// Equal Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery Equal(string fieldName, dynamic value, bool or = false);

        /// <summary>
        /// Equal Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery Equal<T>(Expression<Func<T, dynamic>> field, dynamic value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region Not Equal

        /// <summary>
        /// Not Equal Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery NotEqual(string fieldName, dynamic value, bool or = false);

        /// <summary>
        /// Not Equal Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery NotEqual<T>(Expression<Func<T, dynamic>> field, dynamic value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region LessThan

        /// <summary>
        /// Less Than Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery LessThan(string fieldName, dynamic value, bool or = false);

        /// <summary>
        /// Less Than Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery LessThan<T>(Expression<Func<T, dynamic>> field, dynamic value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region LessThanOrEqual

        /// <summary>
        /// Less Than Or Equal Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery LessThanOrEqual(string fieldName, dynamic value, bool or = false);

        /// <summary>
        /// Less Than Or Equal Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery LessThanOrEqual<T>(Expression<Func<T, dynamic>> field, dynamic value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region GreaterThan

        /// <summary>
        /// Greater Than Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery GreaterThan(string fieldName, dynamic value, bool or = false);

        /// <summary>
        /// Greater Than Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery GreaterThan<T>(Expression<Func<T, dynamic>> field, dynamic value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region GreaterThanOrEqual

        /// <summary>
        /// Greater Than Or Equal Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery GreaterThanOrEqual(string fieldName, dynamic value, bool or = false);

        /// <summary>
        /// Greater Than Or Equal Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery GreaterThanOrEqual<T>(Expression<Func<T, dynamic>> field, dynamic value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region IN

        /// <summary>
        /// Include Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery In(string fieldName, IEnumerable value, bool or = false);

        /// <summary>
        /// Include Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery In<T>(Expression<Func<T, dynamic>> field, IEnumerable value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region Not In

        /// <summary>
        /// Not Include
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery NotIn(string fieldName, IEnumerable value, bool or = false);

        /// <summary>
        /// Not Include
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery NotIn<T>(Expression<Func<T, dynamic>> field, IEnumerable value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region Like

        /// <summary>
        /// Like Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery Like(string fieldName, string value, bool or = false);

        /// <summary>
        /// Like Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery Like<T>(Expression<Func<T, dynamic>> field, string value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region BeginLike

        /// <summary>
        /// Begin Like Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery BeginLike(string fieldName, string value, bool or = false);

        /// <summary>
        /// Begin Like Condition
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery BeginLike<T>(Expression<Func<T, dynamic>> field, string value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region EndLike

        /// <summary>
        /// End Like Condition
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery EndLike(string fieldName, string value, bool or = false);

        /// <summary>
        /// EndLike
        /// </summary>
        /// <param name="field">field</param>
        /// <param name="value">value</param>
        /// <param name="or">connect with 'and'(true/default) or 'or'(false)</param>
        /// <returns>return newest instance</returns>
        IQuery EndLike<T>(Expression<Func<T, dynamic>> field, string value, bool or = false) where T : IQueryModel<T>;

        #endregion

        #region Fields

        /// <summary>
        /// Add Special Fields Need To Query
        /// </summary>
        /// <param name="fields">fields</param>
        /// <returns>return newest instance</returns>
        IQuery AddQueryFields(params string[] fields);

        /// <summary>
        /// Add Special Fields Need To Query
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <typeparam name="TProperty">field type</typeparam>
        /// <param name="fieldExpression">field expression</param>
        /// <returns>return newest instance</returns>
        IQuery AddQueryFields<T>(Expression<Func<T, dynamic>> fieldExpression) where T : IQueryModel<T>;

        /// <summary>
        /// Add Special Fields That don't Query
        /// </summary>
        /// <param name="fields">fields</param>
        /// <returns>return newest instance</returns>
        IQuery AddNotQueryFields(params string[] fields);

        /// <summary>
        /// Add Special Fields That don't Query
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <typeparam name="TProperty">field type</typeparam>
        /// <param name="fieldExpression">field expression</param>
        /// <returns>return newest instance</returns>
        IQuery AddNotQueryFields<T>(Expression<Func<T, dynamic>> fieldExpression) where T : IQueryModel<T>;

        #endregion

        #region ASC

        /// <summary>
        /// Order By ASC
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <typeparam name="TProperty">field type</typeparam>
        /// <param name="field">field</param>
        /// <returns>return newest instance</returns>
        IQuery Asc<T>(Expression<Func<T, dynamic>> field) where T : IQueryModel<T>;

        /// <summary>
        /// Order By ASC
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <returns>return newest instance</returns>
        IQuery Asc(string fieldName);

        #endregion

        #region DESC

        /// <summary>
        /// Order By DESC
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <typeparam name="TProperty">field type</typeparam>
        /// <param name="field">field</param>
        /// <returns>return newest instance</returns>
        IQuery Desc<T>(Expression<Func<T, dynamic>> field) where T : IQueryModel<T>;

        /// <summary>
        /// Order By DESC
        /// </summary>
        /// <param name="fieldName">field</param>
        /// <returns>return newest instance</returns>
        IQuery Desc(string fieldName);

        #endregion

        #region QueryText

        /// <summary>
        /// Set QueryText
        /// </summary>
        /// <param name="queryText">query text</param>
        /// <param name="parameters">parameters</param>
        /// <returns>return newest instance</returns>
        IQuery SetQueryText(string queryText, object parameters = null);

        #endregion

        #region Order Datas

        /// <summary>
        /// Order Datas
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="datas">datas</param>
        /// <returns></returns>
        IEnumerable<T> Order<T>(IEnumerable<T> datas);

        #endregion

        #region Load Propertys

        /// <summary>
        /// Set Load Propertys
        /// </summary>
        /// <param name="propertys">load propertys</param>
        void SetLoadPropertys(Dictionary<string, bool> propertys);

        /// <summary>
        /// Set Load Propertys
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="allowLoad">allow load</param>
        /// <param name="propertys">propertys</param>
        void SetLoadPropertys<T>(bool allowLoad, params Expression<Func<T, dynamic>>[] propertys);

        /// <summary>
        /// property is allow load data
        /// </summary>
        /// <param name="propertyName">propertyName</param>
        /// <returns>allow load data</returns>
        bool AllowLoad(string propertyName);

        /// <summary>
        /// property is allow load data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">propertyName</param>
        /// <returns>allow load data</returns>
        bool AllowLoad<T>(Expression<Func<T, dynamic>> property);

        #endregion

        #endregion
    }
}
