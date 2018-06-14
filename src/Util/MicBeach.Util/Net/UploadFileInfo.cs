using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Net
{
    [Serializable]
    public class UploadFileInfo
    {
        #region Propertys 

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix
        {
            get;
            set;
        }

        /// <summary>
        /// OriginalFileName
        /// </summary>
        public string OriginalFileName
        {
            get; set;
        }

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// RelativePath
        /// </summary>
        public string RelativePath
        {
            get; set;
        }

        /// <summary>
        /// FullPath
        /// </summary>
        public string FullPath
        {
            get;
            set;
        }

        #endregion
    }
}
