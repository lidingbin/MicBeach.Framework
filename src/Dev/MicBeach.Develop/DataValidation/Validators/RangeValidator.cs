using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation.Validators
{
    /// <summary>
    /// 范围验证
    /// </summary>
    public class RangeValidator : DataValidator
    {
        public dynamic Minimum { get; private set; }
        public dynamic Maximum { get; private set; }

        RangeBoundary _lowerBoundary = RangeBoundary.Include;
        RangeBoundary _upperBoundary = RangeBoundary.Include;

        public RangeValidator(dynamic minimum, dynamic maximum,RangeBoundary lowerBoundary=RangeBoundary.Include,RangeBoundary upperBoundary=RangeBoundary.Include)
        {
            Minimum = minimum;
            Maximum = maximum;
            _lowerBoundary = lowerBoundary;
            _upperBoundary = upperBoundary;
            _errorMessage = "值超出范围";
        }

        public override void Validate(dynamic value, string errorMessage)
        {
            _isValid = _lowerBoundary == RangeBoundary.Include ? value >= Minimum : value > Minimum;
            if (_isValid)
            {
                _isValid = _isValid && (_upperBoundary == RangeBoundary.Include ? value <= Maximum : value < Maximum);
            }
            SetVerifyResult(_isValid, errorMessage);
        }

        /// <summary>
        /// 生成验证属性
        /// </summary>
        /// <returns></returns>
        public override ValidationAttribute CreateValidationAttribute(ValidationAttributeParameter parameter)
        {
            return new RangeAttribute(Minimum,Maximum)
            {
                ErrorMessage = FormatMessage(parameter.ErrorMessage)
            };
        }
    }
}
