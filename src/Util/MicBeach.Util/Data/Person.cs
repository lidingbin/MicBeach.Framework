using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Person data type
    /// </summary>
    public class Person
    {
        #region fields

        /// <summary>
        /// person name
        /// </summary>
        ChineseText _name;

        /// <summary>
        /// birth
        /// </summary>
        Birth _birth;

        /// <summary>
        /// contact
        /// </summary>
        Contact _contact;

        Sex _sex;

        /// <summary>
        /// id card
        /// </summary>
        string _idCard;

        #endregion

        #region Propertys

        /// <summary>
        /// get or set name
        /// </summary>
        public ChineseText Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// get or set birth
        /// </summary>
        public Birth Birth
        {
            get
            {
                return _birth;
            }
            set
            {
                _birth = value;
            }
        }

        /// <summary>
        /// get or set contact
        /// </summary>
        public Contact Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
            }
        }

        /// <summary>
        /// get or set sex
        /// </summary>
        public Sex Sex
        {
            get
            {
                return _sex;
            }
            set
            {
                _sex = value;
            }
        }

        /// <summary>
        /// get or set idcard
        /// </summary>
        public string IdCard
        {
            get
            {
                return _idCard;
            }
            set
            {
                _idCard = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// sex
    /// </summary>
    public enum Sex
    {
        男 = 2,
        女 = 4,
        保密 = 8
    }
}
