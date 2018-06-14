using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// Address
    /// </summary>
    public struct Address
    {
        #region fields

        private List<Region> _regionList;//region list
        private string _streetAddress;//street
        private string _zipCode;// zip code

        #endregion

        #region constructor

        /// <summary>
        /// instance a address
        /// </summary>
        /// <param name="streetAddress">street address</param>
        /// <param name="regionList">region list</param>
        /// <param name="zipCode">zip code</param>
        public Address(string streetAddress, List<Region> regionList = null, string zipCode = "")
        {
            _streetAddress = streetAddress;
            _regionList = regionList;
            _zipCode = zipCode;
        }

        #endregion

        #region Propertys

        /// <summary>
        /// get regions
        /// </summary>
        public List<Region> Regions
        {
            get
            {
                return _regionList;
            }
        }

        /// <summary>
        /// get street address
        /// </summary>
        public string StreetAddress
        {
            get
            {
                return _streetAddress;
            }
        }

        /// <summary>
        /// get zip code
        /// </summary>
        public string ZipCode
        {
            get
            {
                return _zipCode;
            }
        }

        #endregion
    }

    /// <summary>
    /// region
    /// </summary>
    public struct Region
    {
        #region fields

        private string _name;//region name
        private string _code;//region codee
        private int _levelIndex;//region leve

        #endregion

        #region constructor

        /// <summary>
        /// instance a Region
        /// </summary>
        /// <param name="name">region name</param>
        /// <param name="code">region code</param>
        public Region(string name, string code = "",int level=0)
        {
            _name = name;
            _code = code;
            _levelIndex = level;
        }

        #endregion

        #region Propertys

        /// <summary>
        /// get region name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// get region code
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
        }

        /// <summary>
        /// get region level
        /// </summary>
        public int Level
        {
            get
            {
                return _levelIndex;
            }
        }

        #endregion
    }
}
