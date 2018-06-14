using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Birth Date Type
    /// </summary>
    public struct Birth
    {
        #region fields

        private DateTime _birthDateTime;//birth datetime
        private int _age;//age
        private Constellation _constellation;//constellation

        #endregion

        #region constructor

        /// <summary>
        /// instance a Birth object
        /// </summary>
        /// <param name="birthDate">birth datetime</param>
        public Birth(DateTime birthDate)
        {
            _birthDateTime = birthDate;
            _constellation = GetConstellation(birthDate);
            _age = GetAge(birthDate);
        }

        #endregion

        #region static methods

        /// <summary>
        /// get constellation
        /// </summary>
        /// <param name="dateTime">datetime</param>
        /// <returns>Constellation</returns>
        public static Constellation GetConstellation(DateTime dateTime)
        {
            return Constellation.双子座;
        }

        /// <summary>
        /// get age
        /// </summary>
        /// <param name="dateTime">birth date</param>
        /// <returns>age</returns>
        public static int GetAge(DateTime dateTime)
        {
            var nowDate = DateTime.Now.Date;
            var birthDate = dateTime.Date;
            if (nowDate < birthDate.AddYears(1))
            {
                return 0;
            }
            return (nowDate - birthDate).Days / 365;
        }

        #endregion
    }

    /// <summary>
    /// Constellation
    /// </summary>
    public enum Constellation
    {
        水瓶座 = 120218,
        双鱼座 = 219320,
        白羊座 = 321419,
        金牛座 = 420520,
        双子座 = 521621,
        巨蟹座 = 622722,
        狮子座 = 723822,
        处女座 = 823922,
        天秤座 = 9231023,
        天蝎座 = 10241122,
        射手座 = 11231221,
        摩羯座 = 12220119
    }
}
