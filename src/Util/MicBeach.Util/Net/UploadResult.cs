using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Net
{
    /// <summary>
    /// Upload Result
    /// </summary>
    [Serializable]
    public class UploadResult
    {
        #region Propertys

        /// <summary>
        /// get or set whether success
        /// </summary>
        public bool Success
        {
            get;
            set;
        }

        /// <summary>
        /// get or set errormsg
        /// </summary>
        public string ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// get or set code
        /// </summary>
        public string Code
        {
            get;set;
        }

        /// <summary>
        /// get or set upload file list
        /// </summary>
        public List<UploadFileInfo> FileInfoList
        {
            get;set;
        }

        #endregion
    }
}
