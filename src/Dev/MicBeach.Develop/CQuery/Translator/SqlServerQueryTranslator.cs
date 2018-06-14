using System;
using System.Collections.Generic;
using System.Text;

namespace MicBeach.Develop.CQuery.Translator
{
    /// <summary>
    /// Query Translator Implement For SqlServer DataBase
    /// </summary>
    public class SqlServerQueryTranslator : IQueryTranslator
    {
        #region Fields

        const string EqualOperator = "=";
        const string GreaterThanOperator = ">";
        const string GreaterThanOrEqualOperator = ">=";
        const string NotEqualOperator = "<>";
        const string LessThanOperator = "<";
        const string LessThanOrEqualOperator = "<=";
        const string InOperator = "IN";
        const string NotInOperator = "NOT IN";
        const string LikeOperator = "LIKE";
        public const string ObjPetName = "TB";
        int subObjectSequence = 0;
        string parameterPrefix = "@";

        #endregion

        #region Propertys

        /// <summary>
        /// Query Object Pet Name
        /// </summary>
        public string ObjectPetName
        {
            get
            {
                return ObjPetName;
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Translate Query Object
        /// </summary>
        /// <param name="query">query object</param>
        /// <returns>translate result</returns>
        public TranslateResult Translate(IQuery query)
        {
            return ExecuteTranslate(query);
        }

        /// <summary>
        /// Execute Translate
        /// </summary>
        /// <param name="query">query object</param>
        /// <param name="paras">parameters</param>
        /// <param name="objectName">query object name</param>
        /// <returns></returns>
        public TranslateResult ExecuteTranslate(IQuery query, Dictionary<string, object> paras = null, string objectName = "", bool subQuery = false)
        {
            if (query == null)
            {
                return TranslateResult.Empty;
            }
            StringBuilder conditionBuilder = new StringBuilder();
            if (query.QueryType == QueryCommandType.QueryObject)
            {
                StringBuilder orderBuilder = new StringBuilder();
                Dictionary<string, object> parameters = paras ?? new Dictionary<string, object>();
                objectName = string.IsNullOrWhiteSpace(objectName) ? ObjPetName : objectName;
                if (query.Criterias != null && query.Criterias.Count > 0)
                {
                    int index = 0;
                    foreach (var queryItem in query.Criterias)
                    {
                        conditionBuilder.AppendFormat("{0} {1}", index > 0 ? " " + queryItem.Item1.ToString() : "", TranslateCondition(queryItem, parameters, objectName));
                        index++;
                    }
                }
                if (!subQuery && query.Orders != null && query.Orders.Count > 0)
                {
                    foreach (var orderItem in query.Orders)
                    {
                        orderBuilder.AppendFormat("{0}.[{1}] {2},", objectName, orderItem.Name, orderItem.Desc ? "DESC" : "ASC");
                    }
                }
                return TranslateResult.CreateNewResult(conditionBuilder.ToString(), orderBuilder.ToString().Trim(','), parameters);
            }
            else
            {
                conditionBuilder.Append(query.QueryText);
                return TranslateResult.CreateNewResult(conditionBuilder.ToString(), string.Empty, query.QueryTextParameters);
            }
        }

        /// <summary>
        /// translate query condition
        /// </summary>
        /// <param name="queryItem">query condition</param>
        /// <returns></returns>
        string TranslateCondition(Tuple<QueryOperator, IQueryItem> queryItem, Dictionary<string, object> parameters, string objectName)
        {
            if (queryItem == null)
            {
                return string.Empty;
            }
            Criteria criteria = queryItem.Item2 as Criteria;
            if (criteria != null)
            {
                return TranslateCriteria(criteria, parameters, objectName);
            }
            IQuery query = queryItem.Item2 as IQuery;
            if (query != null && query.Criterias != null && query.Criterias.Count > 0)
            {
                if (query.Criterias.Count == 1)
                {
                    var firstCriterias = query.Criterias[0];
                    if (firstCriterias.Item2 is Criteria)
                    {
                        return TranslateCriteria(firstCriterias.Item2 as Criteria, parameters, objectName);
                    }
                    return TranslateCondition(firstCriterias, parameters, objectName);
                }
                StringBuilder subCondition = new StringBuilder("(");
                int index = 0;
                foreach (var subQueryItem in query.Criterias)
                {
                    subCondition.AppendFormat("{0} {1}", index > 0 ? " " + subQueryItem.Item1.ToString() : "", TranslateCondition(subQueryItem, parameters, objectName));
                    index++;
                }
                return subCondition.Append(")").ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Translate Single Criteria
        /// </summary>
        /// <param name="criteria">criteria</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        string TranslateCriteria(Criteria criteria, Dictionary<string, object> parameters, string objectName)
        {
            if (criteria == null)
            {
                return string.Empty;
            }
            IQuery valueQuery = criteria.Value as IQuery;
            string parameterName = criteria.Name + parameters.Count.ToString();
            string sqlOperator = GetOperator(criteria.Operator);
            if (valueQuery != null)
            {
                string subObjName = "TSB" + subObjectSequence;
                subObjectSequence++;
                var subQueryResult = ExecuteTranslate(valueQuery, parameters, subObjName, true);
                string topString = "";
                if (sqlOperator != InOperator && sqlOperator != NotInOperator)
                {
                    topString = "TOP 1";
                }
                string conditionString = subQueryResult.ConditionString;
                if (!string.IsNullOrWhiteSpace(conditionString))
                {
                    conditionString = "WHERE " + conditionString;
                }
                return string.Format("{0}.[{1}] {2} (SELECT {3} {4}.[{5}] FROM [{6}] {7} {8} {9})", objectName, criteria.Name, sqlOperator, topString, subObjName, valueQuery.QueryFields[0], valueQuery.ObjectName, subObjName, conditionString, subQueryResult.OrderString);
            }
            parameters.Add(parameterName, FormatCriteriaValue(criteria.Operator, criteria.GetCriteriaRealValue()));
            return string.Format("{0}.[{1}] {2} {4}{3}", objectName, criteria.Name, sqlOperator, parameterName, parameterPrefix);
        }

        /// <summary>
        /// get sql operator by condition operator
        /// </summary>
        /// <param name="criteriaOperator"></param>
        /// <returns></returns>
        string GetOperator(CriteriaOperator criteriaOperator)
        {
            string sqlOperator = string.Empty;
            switch (criteriaOperator)
            {
                case CriteriaOperator.Equal:
                    sqlOperator = EqualOperator;
                    break;
                case CriteriaOperator.GreaterThan:
                    sqlOperator = GreaterThanOperator;
                    break;
                case CriteriaOperator.GreaterThanOrEqual:
                    sqlOperator = GreaterThanOrEqualOperator;
                    break;
                case CriteriaOperator.NotEqual:
                    sqlOperator = NotEqualOperator;
                    break;
                case CriteriaOperator.LessThan:
                    sqlOperator = LessThanOperator;
                    break;
                case CriteriaOperator.LessThanOrEqual:
                    sqlOperator = LessThanOrEqualOperator;
                    break;
                case CriteriaOperator.In:
                    sqlOperator = InOperator;
                    break;
                case CriteriaOperator.NotIn:
                    sqlOperator = NotInOperator;
                    break;
                case CriteriaOperator.Like:
                case CriteriaOperator.BeginLike:
                case CriteriaOperator.EndLike:
                    sqlOperator = LikeOperator;
                    break;
            }
            return sqlOperator;
        }

        /// <summary>
        /// Format Value
        /// </summary>
        /// <param name="criteriaOperator">condition operator</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        dynamic FormatCriteriaValue(CriteriaOperator criteriaOperator, dynamic value)
        {
            dynamic realValue = value;
            switch (criteriaOperator)
            {
                case CriteriaOperator.Like:
                    realValue = string.Format("%{0}%", value);
                    break;
                case CriteriaOperator.BeginLike:
                    realValue = string.Format("{0}%", value);
                    break;
                case CriteriaOperator.EndLike:
                    realValue = string.Format("%{0}", value);
                    break;
            }
            return realValue;
        }

        #endregion
    }
}
