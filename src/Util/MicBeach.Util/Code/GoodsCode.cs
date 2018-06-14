using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MicBeach.Util.Code
{
    /// <summary>
    /// generate china goods code
    /// </summary>
    public static class GoodsCode
    {
        /// <summary>
        /// get a china goods code
        /// codePrefix is a positive number and length must be euqal to 8
        /// codeStartIndex mast be between 0 to 9999
        /// </summary>
        /// <param name="codePrefix">code prefix</param>
        /// <param name="codeStartIndex">start index</param>
        /// <param name="count">generate code count</param>
        /// <returns>code list</returns>
        public static List<string> GetCodeList(int codePrefix, int codeStartIndex, int count = 1)
        {
            #region args verify

            if (codeStartIndex < 0 || codeStartIndex > 9999)
            {
                throw new Exception("codeStartIndex mast be between 0 to 9999");
            }

            #endregion

            List<string> codeValues = new List<string>();
            for (int codeNum = codeStartIndex; codeNum <= 9999 && codeValues.Count < count; codeNum++)
            {
                codeValues.Add(GetCode(codePrefix, codeNum));
            }
            return codeValues;
        }

        /// <summary>
        /// get a china goods code
        /// codePrefix is a positive number and length must be euqal to 8
        /// codeIndex mast be between 0 to 9999
        /// </summary>
        /// <param name="codePrefix">code prefix</param>
        /// <param name="codeIndex">code index</param>
        /// <returns>goods code</returns>
        public static string GetCode(int codePrefix, int codeIndex)
        {
            #region args verify

            if (codePrefix <= 0)
            {
                throw new Exception("codePrefix is a positive number and length must be euqal to 8");
            }
            string codePrefixString = codePrefix.ToString();
            if (codePrefixString.Length != 8)
            {
                throw new Exception("codePrefix is a positive number and length must be euqal to 8");
            }
            if (codeIndex < 0 || codeIndex > 9999)
            {
                throw new Exception("codeIndex mast be between 0 to 9999");
            }

            #endregion

            string codeIndexString = codeIndex.ToString();
            codeIndexString = string.Format("{0}{1}", new string('0', 4 - codeIndexString.Length), codeIndexString);
            int evenNumberSum = 0;
            List<int> evenNumberValues = new List<int>();
            for (int i = 1; i < codePrefixString.Length; i += 2)
            {
                evenNumberValues.Add(int.Parse(codePrefixString[i].ToString()));
            }
            for (int i = 1; i < codeIndexString.Length; i += 2)
            {
                evenNumberValues.Add(int.Parse(codeIndexString[i].ToString()));
            }
            evenNumberSum = evenNumberValues.Sum() * 3;

            int oddNumberSum = 0;
            List<int> oddNumberValues = new List<int>();
            for (int i = 0; i < codePrefixString.Length; i += 2)
            {
                oddNumberValues.Add(int.Parse(codePrefixString[i].ToString()));
            }
            for (int i = 0; i < codeIndexString.Length; i += 2)
            {
                oddNumberValues.Add(int.Parse(codeIndexString[i].ToString()));
            }
            oddNumberSum = oddNumberValues.Sum();
            oddNumberSum += evenNumberSum;
            string lastCode = "0";
            int remainder = oddNumberSum % 10;
            if (remainder != 0)
            {
                lastCode = (10 - remainder).ToString();
            }
            return string.Format("{0}{1}{2}", codePrefix, codeIndexString, lastCode);
        }
    }
}
