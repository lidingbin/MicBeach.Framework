using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation
{
    /// <summary>
    /// 比较运算符
    /// </summary>
    public enum CompareOperator
    {
        Equal,
        NotEqual,
        LessThanOrEqual,
        LessThan,
        GreaterThan,
        GreaterThanOrEqual,
        In,
        NotIn,
    }

    /// <summary>
    /// 验证范围边界
    /// </summary>
    public enum RangeBoundary
    {
        Include,
        NotInclude
    }

    /// <summary>
    /// 验证器类型
    /// </summary>
    public enum ValidatorType
    {
        Compare,
        CreditCard,
        Email,
        EnumType,
        MaxLength,
        MinLength,
        Phone,
        Range,
        RegularExpression,
        Required,
        StringLength,
        Url
    }
}
