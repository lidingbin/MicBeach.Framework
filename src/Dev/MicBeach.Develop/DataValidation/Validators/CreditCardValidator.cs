﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation.Validators
{
    /// <summary>
    /// 信用卡验证
    /// </summary>
    public class CreditCardValidator : DataValidator
    {
        public CreditCardValidator()
        {
            _errorMessage = "信用卡格式不正确";
        }
        public override void Validate(dynamic value, string errorMessage)
        {
            if (value == null)
            {
                SetVerifyResult(false, errorMessage);
                return;
            }
            string ccValue = value as string;
            if (ccValue == null)
            {
                SetVerifyResult(false, errorMessage);
                return;
            }
            ccValue = ccValue.Replace("-", "");
            ccValue = ccValue.Replace(" ", "");
            int checksum = 0;
            bool evenDigit = false;
            foreach (char digit in ccValue.Reverse())
            {
                if (digit < '0' || digit > '9')
                {
                    SetVerifyResult(false, errorMessage);
                    return;
                }
                int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                evenDigit = !evenDigit;
                while (digitValue > 0)
                {
                    checksum += digitValue % 10;
                    digitValue /= 10;
                }
            }
            _isValid = (checksum % 10) == 0;
            SetVerifyResult(_isValid, errorMessage);
        }

        /// <summary>
        /// 生成验证属性
        /// </summary>
        /// <returns></returns>
        public override ValidationAttribute CreateValidationAttribute(ValidationAttributeParameter parameter)
        {
            return new CreditCardAttribute()
            {
                ErrorMessage = FormatMessage(parameter.ErrorMessage)
            };
        }
    }
}
