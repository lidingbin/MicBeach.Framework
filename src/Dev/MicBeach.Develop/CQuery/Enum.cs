using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.CQuery
{
    /// <summary>
    /// 条件运算符
    /// </summary>
    public enum CriteriaOperator
    {
        Equal,              //=  
        NotEqual,      //<>  
        LessThanOrEqual,    //<=  
        LessThan,           //<  
        GreaterThan,        //>  
        GreaterThanOrEqual, //>=  
        In,                 //IN()  
        NotIn,              //NOT IN ()  
        Like,
        BeginLike,
        EndLike
    }

    /// <summary>
    /// 条件连接符
    /// </summary>
    public enum QueryOperator
    {
        AND,
        OR
    }

    /// <summary>
    /// 查询命令类型
    /// </summary>
    public enum QueryCommandType
    {
        QueryObject,
        Text
    }

    /// <summary>
    /// 运算符号
    /// </summary>
    public enum CalculateOperator
    {
        Add,
        subtract,
        multiply,
        divide
    }
}
