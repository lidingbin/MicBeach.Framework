﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Develop.DataValidation
{
    public static class ValidationExtension
    {
        /// <summary>
        /// 获取验证结果中错误消息
        /// </summary>
        /// <param name="results">验证结果</param>
        /// <returns></returns>
        public static string[] GetErrorMessage(this IEnumerable<VerifyResult> results)
        {
            if (results == null)
            {
                return new string[0];
            }
            List<string> errorMessages = new List<string>();
            foreach (var result in results)
            {
                if (!result.Success)
                {
                    errorMessages.Add(string.Format("{0}/{1}",result.FieldName,result.ErrorMessage));
                }
            }
            return errorMessages.ToArray();
        }
    }
}
