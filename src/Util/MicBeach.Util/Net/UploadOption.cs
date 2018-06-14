using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Net
{
    /// <summary>
    /// UploadOption
    /// </summary>
    public class UploadOption
    {
        #region Propertys

        /// <summary>
        /// Folder
        /// </summary>
        public string Folder
        {
            get;set;
        }

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix
        {
            get;set;
        }

        /// <summary>
        /// RenameFile
        /// </summary>
        public bool RenameFile
        {
            get;set;
        }

        /// <summary>
        /// JsonData
        /// </summary>
        public string JsonData
        {
            get;set;
        }

        #endregion
    }
}
